using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Skills;

public sealed class AddUserSkillRequest
{
    [Required]
    public required Guid SkillId { get; set; }

    /// <summary>Beginner, Intermediate, Advanced o Expert.</summary>
    [Required]
    public required string Level { get; set; }

    public bool IsSpeakerSkill { get; set; }
    public bool IsMentorSkill { get; set; }
}
