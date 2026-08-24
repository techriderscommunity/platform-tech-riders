namespace TechRiders.Domain.Entities;

public sealed class KnowledgeArticleCategory
{
    public Guid KnowledgeArticleId { get; set; }
    public KnowledgeArticle KnowledgeArticle { get; set; } = default!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}
