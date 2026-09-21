using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

public interface IKnowledgeArticleRepository
{
    Task<KnowledgeArticle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<KnowledgeArticle?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<KnowledgeArticle> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? categoryId,
        Guid? skillId,
        string? search,
        string? categoryName = null,
        CancellationToken cancellationToken = default);

    Task<Category?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken = default);

    Task<Skill?> GetSkillByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Guid?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<Guid?> GetStatusIdAsync(string scope, string name, CancellationToken cancellationToken = default);

    Task<KnowledgeArticle> AddAsync(KnowledgeArticle article, CancellationToken cancellationToken = default);

    Task UpdateAsync(KnowledgeArticle article, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca por slug sin filtrar por IsActive (upsert de import, puede reactivar un art\u00edculo).
    /// </summary>
    Task<KnowledgeArticle?> FindAnyBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
