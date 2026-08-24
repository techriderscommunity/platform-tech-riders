using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

public sealed class UserSkill
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = default!;

    public SkillLevel Level { get; set; } = SkillLevel.Beginner;
    public bool IsSpeakerSkill { get; set; }
    public bool IsMentorSkill { get; set; }
}
