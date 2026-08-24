namespace TechRiders.Domain.Entities;

public sealed class Skill : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public Guid? ParentSkillId { get; set; }
    public Skill? ParentSkill { get; set; }
    public ICollection<Skill> Children { get; set; } = new List<Skill>();

    public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
    public ICollection<SessionSkill> SessionSkills { get; set; } = new List<SessionSkill>();
    public ICollection<KnowledgeArticleSkill> KnowledgeArticleSkills { get; set; } = new List<KnowledgeArticleSkill>();
}
