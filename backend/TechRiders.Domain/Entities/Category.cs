namespace TechRiders.Domain.Entities;

public sealed class Category : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }

    public ICollection<UserCategory> UserCategories { get; set; } = new List<UserCategory>();
    public ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();
    public ICollection<SessionCategory> SessionCategories { get; set; } = new List<SessionCategory>();
    public ICollection<KnowledgeArticleCategory> KnowledgeArticleCategories { get; set; } = new List<KnowledgeArticleCategory>();
}
