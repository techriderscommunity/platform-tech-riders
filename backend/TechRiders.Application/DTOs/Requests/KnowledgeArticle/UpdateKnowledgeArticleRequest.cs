namespace TechRiders.Application.DTOs.Requests.KnowledgeArticle;

public sealed class UpdateKnowledgeArticleRequest
{
    public required string Title { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public List<string> CategoryNames { get; set; } = new();
    public List<string> SkillNames { get; set; } = new();
}
