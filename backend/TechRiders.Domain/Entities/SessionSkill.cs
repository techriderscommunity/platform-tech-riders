namespace TechRiders.Domain.Entities;

public sealed class SessionSkill
{
    public Guid SessionId { get; set; }
    public Session Session { get; set; } = default!;

    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = default!;
}
