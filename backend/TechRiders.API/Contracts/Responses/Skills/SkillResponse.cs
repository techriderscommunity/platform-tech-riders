namespace TechRiders.Api.Contracts.Responses.Skills;

public sealed class SkillResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid? ParentSkillId { get; set; }
}

public sealed class UserSkillResponse
{
    public required Guid SkillId { get; set; }
    public required string SkillName { get; set; }
    public required string Level { get; set; }
    public required bool IsSpeakerSkill { get; set; }
    public required bool IsMentorSkill { get; set; }
}
