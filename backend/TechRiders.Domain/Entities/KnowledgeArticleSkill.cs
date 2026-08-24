namespace TechRiders.Domain.Entities;

public sealed class KnowledgeArticleSkill
{
    public Guid KnowledgeArticleId { get; set; }
    public KnowledgeArticle KnowledgeArticle { get; set; } = default!;

    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = default!;
}
