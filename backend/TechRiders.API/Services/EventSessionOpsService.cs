using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>
/// Operaciones administrativas de Eventos/Sesiones que complementan los CRUD ya existentes
/// (EventService/SessionService): publicar/cancelar (v\u00eda cat\u00e1logo Status gen\u00e9rico), asignar
/// ponentes, sincronizar skills y gestionar inscripciones. "Aprobar" una sesi\u00f3n reutiliza la misma
/// transici\u00f3n de estado que "publicar" (no hay un paso de revisi\u00f3n separado en el modelo actual).
/// </summary>
public static class EventSessionOpsService
{
    public const string PublishedStatusName = "Publicado";
    public const string CancelledStatusName = "Cancelado";

    public static Task SetEventStatusAsync(TechRidersDbContext dbContext, Guid eventId, string statusName, CancellationToken cancellationToken = default) =>
        SetStatusAsync(dbContext, statusName, "Event", async status =>
        {
            var evento = await dbContext.Set<Event>().FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken)
                ?? throw new InvalidOperationException("Evento no encontrado.");
            evento.StatusId = status.Id;
            evento.UpdatedAt = DateTime.UtcNow;
        }, cancellationToken);

    public static Task SetSessionStatusAsync(TechRidersDbContext dbContext, Guid sessionId, string statusName, CancellationToken cancellationToken = default) =>
        SetStatusAsync(dbContext, statusName, "Session", async status =>
        {
            var session = await dbContext.Set<Session>().FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken)
                ?? throw new InvalidOperationException("Sesi\u00f3n no encontrada.");
            session.StatusId = status.Id;
            session.UpdatedAt = DateTime.UtcNow;
        }, cancellationToken);

    private static async Task SetStatusAsync(TechRidersDbContext dbContext, string statusName, string scope, Func<Status, Task> apply, CancellationToken cancellationToken)
    {
        var status = await dbContext.Set<Status>().FirstOrDefaultAsync(s => s.Name == statusName && s.Scope == scope, cancellationToken);
        if (status is null)
        {
            status = new Status { Id = Guid.NewGuid(), Name = statusName, Scope = scope };
            await dbContext.Set<Status>().AddAsync(status, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        await apply(status);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public static async Task AddSpeakerAsync(TechRidersDbContext dbContext, Guid sessionId, Guid userId, bool isMainSpeaker, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.Set<SessionSpeaker>().AnyAsync(s => s.SessionId == sessionId && s.UserId == userId, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("La persona ya est\u00e1 asignada como ponente de esta sesi\u00f3n.");
        }

        await dbContext.Set<SessionSpeaker>().AddAsync(new SessionSpeaker { SessionId = sessionId, UserId = userId, IsMainSpeaker = isMainSpeaker }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public static async Task RemoveSpeakerAsync(TechRidersDbContext dbContext, Guid sessionId, Guid userId, CancellationToken cancellationToken = default)
    {
        var speaker = await dbContext.Set<SessionSpeaker>().FirstOrDefaultAsync(s => s.SessionId == sessionId && s.UserId == userId, cancellationToken)
            ?? throw new InvalidOperationException("Ponente no encontrado en esta sesi\u00f3n.");

        dbContext.Set<SessionSpeaker>().Remove(speaker);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public static async Task SyncSkillsAsync(TechRidersDbContext dbContext, Guid sessionId, IReadOnlyCollection<Guid> skillIds, CancellationToken cancellationToken = default)
    {
        var current = await dbContext.Set<SessionSkill>().Where(s => s.SessionId == sessionId).ToListAsync(cancellationToken);
        dbContext.Set<SessionSkill>().RemoveRange(current.Where(c => !skillIds.Contains(c.SkillId)));

        var existingIds = current.Select(c => c.SkillId).ToHashSet();
        foreach (var skillId in skillIds.Where(id => !existingIds.Contains(id)))
        {
            await dbContext.Set<SessionSkill>().AddAsync(new SessionSkill { SessionId = sessionId, SkillId = skillId }, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public static Task<List<EventRegistration>> GetEventRegistrationsAsync(TechRidersDbContext dbContext, Guid eventId, CancellationToken cancellationToken = default) =>
        dbContext.Set<EventRegistration>().Include(r => r.User).Where(r => r.EventId == eventId).ToListAsync(cancellationToken);

    public static async Task UpdateEventRegistrationStatusAsync(TechRidersDbContext dbContext, Guid registrationId, string status, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<RegistrationStatus>(status, ignoreCase: true, out var parsedStatus))
        {
            throw new ArgumentException($"Estado de inscripci\u00f3n '{status}' no reconocido.");
        }

        var registration = await dbContext.Set<EventRegistration>().FirstOrDefaultAsync(r => r.Id == registrationId, cancellationToken)
            ?? throw new InvalidOperationException("Inscripci\u00f3n no encontrada.");

        registration.RegistrationStatus = parsedStatus;
        registration.Attended = parsedStatus == RegistrationStatus.Attended;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
