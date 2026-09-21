using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>
/// Sugerencia de candidatos para asignar a una iniciativa (sesion/evento/FPTour), combinando
/// UserSkill + UserPreference (dimension Disponibilidad) + historico de participacion. Solo
/// lectura: la asignacion final se realiza llamando a los endpoints ya existentes.
/// </summary>
public sealed class AssignmentService : IAssignmentService
{
    private readonly TechRidersDbContext _dbContext;

    public AssignmentService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<AssignmentCandidate>> GetCandidatesAsync(Guid? skillId, Guid? availabilityValueId, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Users.Where(u => u.IsActive).AsQueryable();

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
