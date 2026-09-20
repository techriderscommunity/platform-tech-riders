using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Sessions;

public sealed class AddSpeakerRequest
{
    [Required]
    public required Guid UserId { get; set; }

    public bool IsMainSpeaker { get; set; }
}

public sealed class SyncSessionSkillsRequest
{
    [Required]
    public required IReadOnlyCollection<Guid> SkillIds { get; set; }
}
