using MapsterMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TechRiders.Application.DTOs.Requests.KnowledgeArticle;
using TechRiders.Application.DTOs.Responses.KnowledgeArticle;
using TechRiders.Application.Exceptions;
using TechRiders.Application.Interfaces;
using TechRiders.Application.KnowledgeArticles;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

public class KnowledgeArticleService : IKnowledgeArticleService
{
    private static readonly TimeSpan ContentCacheDuration = TimeSpan.FromMinutes(15);

    private readonly IUnitOfWork _unitOfWork;
    private readonly IKnowledgeContentBlobService _blobService;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;
    private readonly ILogger<KnowledgeArticleService> _logger;

    public KnowledgeArticleService(
        IUnitOfWork unitOfWork,
        IKnowledgeContentBlobService blobService,
        IMemoryCache cache,
        IMapper mapper,
        ILogger<KnowledgeArticleService> logger)
    {
        _unitOfWork = unitOfWork;
        _blobService = blobService;
        _cache = cache;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedKnowledgeArticleResponse> GetPagedAsync(
        int page,
        int pageSize,
        Guid? categoryId,
        Guid? skillId,
        string? search,
        string? categoryName = null,
        CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _unitOfWork.KnowledgeArticles.GetPagedAsync(page, pageSize, categoryId, skillId, search, categoryName, cancellationToken);

        return new PagedKnowledgeArticleResponse
        {
            Items = _mapper.Map<List<KnowledgeArticleSummaryResponse>>(items),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<KnowledgeArticleResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var article = await _unitOfWork.KnowledgeArticles.GetBySlugAsync(slug, cancellationToken);
        if (article is null) return null;

        var response = _mapper.Map<KnowledgeArticleResponse>(article);
        response.ContentMd = await ReadContentWithCacheAsync(article.ContentMd, cancellationToken);
        return response;
    }

    private async Task<string> ReadContentWithCacheAsync(string blobPath, CancellationToken cancellationToken)
    {
        return await _cache.GetOrCreateAsync(CacheKey(blobPath), async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = ContentCacheDuration;
            try
            {
                var exists = await _blobService.ExistsAsync(blobPath, cancellationToken);
                if (!exists)
                {
                    throw new KnowledgeContentUnavailableException(blobPath);
                }

                return await _blobService.ReadContentAsync(blobPath, cancellationToken);
            }
            catch (KnowledgeContentUnavailableException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo leyendo el blob {BlobPath}", blobPath);
                throw new KnowledgeContentUnavailableException(blobPath, ex);
            }
        }) ?? throw new KnowledgeContentUnavailableException(blobPath);
    }

    private static string CacheKey(string blobPath) => $"knowledge-content:{blobPath}";

    public async Task<KnowledgeArticleResponse?> UpdateAsync(Guid id, UpdateKnowledgeArticleRequest request, CancellationToken cancellationToken = default)
    {
        var article = await _unitOfWork.KnowledgeArticles.GetByIdAsync(id, cancellationToken);
        if (article is null || !article.IsActive) return null;

        article.Title = request.Title;
        article.PublishedAt = request.PublishedAt;
        article.UpdatedAt = DateTime.UtcNow;

        await ReplaceCategoriesAsync(article, request.CategoryNames, cancellationToken);
        await ReplaceSkillsAsync(article, request.SkillNames, cancellationToken);

        await _unitOfWork.KnowledgeArticles.UpdateAsync(article, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _cache.Remove(CacheKey(article.ContentMd));

        var response = _mapper.Map<KnowledgeArticleResponse>(article);
        response.ContentMd = await ReadContentWithCacheAsync(article.ContentMd, cancellationToken);
        return response;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _unitOfWork.KnowledgeArticles.GetByIdAsync(id, cancellationToken);
        if (article is null || !article.IsActive) return false;

        article.IsActive = false;
        article.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.KnowledgeArticles.UpdateAsync(article, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<KnowledgeArticleImportResultResponse> ImportAsync(KnowledgeArticleImportRequest request, CancellationToken cancellationToken = default)
    {
        var result = new KnowledgeArticleImportResultResponse { Requested = request.Items.Count };

        var authorUserId = await _unitOfWork.KnowledgeArticles.GetUserIdByEmailAsync(KnowledgeArticleDefaults.MigrationAuthorEmail, cancellationToken);
        var statusId = await _unitOfWork.KnowledgeArticles.GetStatusIdAsync(KnowledgeArticleDefaults.StatusScope, KnowledgeArticleDefaults.PublishedStatusName, cancellationToken);

        foreach (var item in request.Items)
        {
            var itemResult = new KnowledgeArticleImportItemResult { Slug = item.Slug };
            try
            {
                if (authorUserId is null)
                {
                    throw new InvalidOperationException($"No existe el usuario de migraci\u00f3n '{KnowledgeArticleDefaults.MigrationAuthorEmail}'. Ejecuta el seed antes de importar.");
                }

                if (statusId is null)
                {
                    throw new InvalidOperationException($"No existe el Status '{KnowledgeArticleDefaults.PublishedStatusName}' (scope {KnowledgeArticleDefaults.StatusScope}). Ejecuta el seed antes de importar.");
                }

                var blobExists = await _blobService.ExistsAsync(item.ContentBlobPath, cancellationToken);
                if (!blobExists)
                {
                    throw new InvalidOperationException($"El blob '{item.ContentBlobPath}' no existe todav\u00eda. Sube el .md al container 'knowledge' y reintenta.");
                }

                var categoryIds = await ResolveCategoryIdsAsync(item.CategoryNames, cancellationToken);
                var skillIds = await ResolveSkillIdsAsync(item.SkillNames, cancellationToken);

                var existing = await _unitOfWork.KnowledgeArticles.FindAnyBySlugAsync(item.Slug, cancellationToken);
                if (existing is null)
                {
                    var article = new KnowledgeArticle
                    {
                        Id = Guid.NewGuid(),
                        Title = item.Title,
                        Slug = item.Slug,
                        ContentMd = item.ContentBlobPath,
                        PublishedAt = item.PublishedAt,
                        AuthorUserId = authorUserId.Value,
                        StatusId = statusId,
                        IsActive = true
                    };
                    foreach (var categoryId in categoryIds)
                    {
                        article.Categories.Add(new KnowledgeArticleCategory { CategoryId = categoryId });
                    }
                    foreach (var skillId in skillIds)
                    {
                        article.Skills.Add(new KnowledgeArticleSkill { SkillId = skillId });
                    }

                    await _unitOfWork.KnowledgeArticles.AddAsync(article, cancellationToken);
                }
                else
                {
                    existing.Title = item.Title;
                    existing.ContentMd = item.ContentBlobPath;
                    existing.PublishedAt = item.PublishedAt;
                    existing.StatusId = statusId;
                    existing.IsActive = true;
                    existing.UpdatedAt = DateTime.UtcNow;
                    existing.Categories.Clear();
                    existing.Skills.Clear();
                    foreach (var categoryId in categoryIds)
                    {
                        existing.Categories.Add(new KnowledgeArticleCategory { CategoryId = categoryId });
                    }
                    foreach (var skillId in skillIds)
                    {
                        existing.Skills.Add(new KnowledgeArticleSkill { SkillId = skillId });
                    }

                    await _unitOfWork.KnowledgeArticles.UpdateAsync(existing, cancellationToken);
                    _cache.Remove(CacheKey(existing.ContentMd));
                }

                itemResult.Imported = true;
            }
            catch (Exception ex)
            {
                itemResult.Imported = false;
                itemResult.Error = ex.Message;
                _logger.LogWarning(ex, "Import rechazado para slug {Slug}", item.Slug);
            }

            result.Items.Add(itemResult);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        result.Imported = result.Items.Count(i => i.Imported);
        result.Rejected = result.Items.Count(i => !i.Imported);
        return result;
    }

    private async Task<List<Guid>> ResolveCategoryIdsAsync(List<string> categoryNames, CancellationToken cancellationToken)
    {
        var resolved = new List<Guid>();
        foreach (var name in categoryNames)
        {
            if (string.IsNullOrWhiteSpace(name)) continue;
            var cleanName = name.Trim();
            var category = await _unitOfWork.KnowledgeArticles.GetCategoryByNameAsync(cleanName, cancellationToken);
            if (category is null)
            {
                category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = cleanName,
                    IsActive = true
                };
                await _unitOfWork.KnowledgeArticles.AddCategoryAsync(category, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            resolved.Add(category.Id);
        }

        if (resolved.Count == 0)
        {
            var fallback = await _unitOfWork.KnowledgeArticles.GetCategoryByNameAsync(KnowledgeArticleDefaults.FallbackCategoryName, cancellationToken);
            if (fallback is not null) resolved.Add(fallback.Id);
        }

        return resolved;
    }

    private async Task<List<Guid>> ResolveSkillIdsAsync(List<string> skillNames, CancellationToken cancellationToken)
    {
        var resolved = new List<Guid>();
        foreach (var name in skillNames)
        {
            var skill = await _unitOfWork.KnowledgeArticles.GetSkillByNameAsync(name, cancellationToken);
            if (skill is not null) resolved.Add(skill.Id);
        }

        return resolved;
    }

    private Task ReplaceCategoriesAsync(KnowledgeArticle article, List<string> categoryNames, CancellationToken cancellationToken)
        => ReplaceAsync(article, categoryNames, ResolveCategoryIdsAsync, ids =>
        {
            article.Categories.Clear();
            foreach (var id in ids) article.Categories.Add(new KnowledgeArticleCategory { CategoryId = id });
        }, cancellationToken);

    private Task ReplaceSkillsAsync(KnowledgeArticle article, List<string> skillNames, CancellationToken cancellationToken)
        => ReplaceAsync(article, skillNames, ResolveSkillIdsAsync, ids =>
        {
            article.Skills.Clear();
            foreach (var id in ids) article.Skills.Add(new KnowledgeArticleSkill { SkillId = id });
        }, cancellationToken);

    private static async Task ReplaceAsync(
        KnowledgeArticle article,
        List<string> names,
        Func<List<string>, CancellationToken, Task<List<Guid>>> resolve,
        Action<List<Guid>> apply,
        CancellationToken cancellationToken)
    {
        var ids = await resolve(names, cancellationToken);
        apply(ids);
    }
}
