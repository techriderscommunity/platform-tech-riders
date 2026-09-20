using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

public sealed record AssignmentCandidate(
    Guid UserId, string Name, string Email, string? SkillLevel,
    bool HasRequestedAvailability, int SessionsAsSpeaker, int EventsParticipated, int FPToursAsAmbassador);

/// <summary>
/// Sugerencia de candidatos para asignar a una iniciativa (sesi\u00f3n/evento/FPTour), combinando
/// UserSkill + UserPreference (dimensi\u00f3n Disponibilidad) + hist\u00f3rico de participaci\u00f3n. Solo
/// lectura: la asignaci\u00f3n final se realiza llamando a los endpoints ya existentes (ponentes,
/// inscripciones, FPTour.AmbassadorUserId) y sigue siendo una decisi\u00f3n manual de Staff/Admin.
/// </summary>
public static class AssignmentSuggestionService
{
    public static async Task<List<AssignmentCandidate>> GetCandidatesAsync(
        TechRidersDbContext dbContext, Guid? skillId, Guid? availabilityValueId, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users.Where(u => u.IsActive).AsQueryable();

        if (skillId.HasValue)
        {
            query = query.Where(u => u.UserSkills.Any(us => us.SkillId == skillId.Value));
        }

        if (availabilityValueId.HasValue)
        {
            query = query.Where(u => u.Preferences.Any(p => p.DimensionValueId == availabilityValueId.Value && p.Status == PreferenceStatus.Activa));
        }

        var users = await query
            .Select(u => new
            {
                u.Id,
                Name = u.Name + " " + u.LastName,
                u.Email,
                SkillLevel = skillId.HasValue ? u.UserSkills.Where(us => us.SkillId == skillId.Value).Select(us => us.Level.ToString()).FirstOrDefault() : null,
                HasAvailability = availabilityValueId.HasValue && u.Preferences.Any(p => p.DimensionValueId == availabilityValueId.Value && p.Status == PreferenceStatus.Activa),
                SpeakerCount = u.SpeakerSessions.Count,
                EventCount = u.EventRegistrations.Count,
                FPTourCount = u.FPTours.Count,
            })
            .ToListAsync(cancellationToken);

        return users
            .Select(u => new AssignmentCandidate(u.Id, u.Name, u.Email, u.SkillLevel, u.HasAvailability, u.SpeakerCount, u.EventCount, u.FPTourCount))
            .OrderByDescending(c => c.SessionsAsSpeaker + c.EventsParticipated + c.FPToursAsAmbassador)
            .ToList();
    }
}
