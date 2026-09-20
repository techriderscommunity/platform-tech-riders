namespace TechRiders.Application.Interfaces;

public sealed record AssignmentCandidate(
    Guid UserId, string Name, string Email, string? SkillLevel,
    bool HasRequestedAvailability, int SessionsAsSpeaker, int EventsParticipated, int FPToursAsAmbassador);

/// <summary>Sugerencia de candidatos para asignar a una iniciativa (sesion/evento/FPTour). Solo lectura.</summary>
public interface IAssignmentService
{
    Task<List<AssignmentCandidate>> GetCandidatesAsync(Guid? skillId, Guid? availabilityValueId, CancellationToken cancellationToken = default);
}
