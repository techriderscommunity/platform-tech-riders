namespace TechRiders.Api.Contracts.Responses.Assignments;

public sealed class AssignmentCandidateResponse
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? SkillLevel { get; set; }
    public required bool HasRequestedAvailability { get; set; }
    public required int SessionsAsSpeaker { get; set; }
    public required int EventsParticipated { get; set; }
    public required int FPToursAsAmbassador { get; set; }
}
