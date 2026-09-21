using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

public class KnowledgeArticleRepository : IKnowledgeArticleRepository
{
    private readonly TechRidersDbContext _context;
    private readonly DbSet<KnowledgeArticle> _dbSet;

    public KnowledgeArticleRepository(TechRidersDbContext context)
    {
        _context = context;
        _dbSet = context.Set<KnowledgeArticle>();
    }

    private IQueryable<KnowledgeArticle> WithGraph()
    {
        return _dbSet
            .Include(a => a.Author)
            .Include(a => a.Status)
            .Include(a => a.Categories).ThenInclude(c => c.Category)
            .Include(a => a.Skills).ThenInclude(s => s.Skill);
    }

    public async Task<KnowledgeArticle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await WithGraph().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<KnowledgeArticle?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await WithGraph().FirstOrDefaultAsync(a => a.Slug == slug && a.IsActive, cancellationToken);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(a => a.Slug == slug && (excludeId == null || a.Id != excludeId), cancellationToken);
    }

    public async Task<(IReadOnlyList<KnowledgeArticle> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? categoryId,
        Guid? skillId,
        string? search,
        string? categoryName = null,
        CancellationToken cancellationToken = default)
    {
        var query = WithGraph().Where(a => a.IsActive);

        if (categoryId.HasValue)
        {
            query = query.Where(a => a.Categories.Any(c => c.CategoryId == categoryId.Value));
        }

        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            query = query.Where(a => a.Categories.Any(c => c.Category.Name == categoryName));
        }

        if (skillId.HasValue)
        {
            query = query.Where(a => a.Skills.Any(s => s.SkillId == skillId.Value));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(a => a.Title.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(a => a.PublishedAt ?? a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Category?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Category>()
            .FirstOrDefaultAsync(c => c.IsActive && c.Name == name, cancellationToken);
    }

    public async Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Set<Category>().AddAsync(category, cancellationToken);
        return category;
    }

    public async Task<Skill?> GetSkillByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Skill>()
            .FirstOrDefaultAsync(s => s.IsActive && s.Name == name, cancellationToken);
    }

    public async Task<Guid?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Email == email)
            .Select(u => (Guid?)u.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid?> GetStatusIdAsync(string scope, string name, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Status>()
            .Where(s => s.Scope == scope && s.Name == name)
            .Select(s => (Guid?)s.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<KnowledgeArticle> AddAsync(KnowledgeArticle article, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(article, cancellationToken);
        return article;
    }

    public Task UpdateAsync(KnowledgeArticle article, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(article);
        return Task.CompletedTask;
    }

    public async Task<KnowledgeArticle?> FindAnyBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await WithGraph().FirstOrDefaultAsync(a => a.Slug == slug, cancellationToken);
    }
}
