namespace TechRiders.Domain.Entities;

public sealed class KnowledgeArticle : BaseEntity
{
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public required string ContentMd { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }

    public Guid AuthorUserId { get; set; }
    public User Author { get; set; } = default!;

    public Guid? StatusId { get; set; }
    public Status? Status { get; set; }

    public ICollection<KnowledgeArticleCategory> Categories { get; set; } = new List<KnowledgeArticleCategory>();
    public ICollection<KnowledgeArticleSkill> Skills { get; set; } = new List<KnowledgeArticleSkill>();
}
