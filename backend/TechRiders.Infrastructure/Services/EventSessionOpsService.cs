using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>
/// Operaciones administrativas de Eventos/Sesiones que complementan los CRUD ya existentes
/// (IEventService/ISessionService): publicar/cancelar (via catalogo Status generico), asignar
/// ponentes, sincronizar skills y gestionar inscripciones. "Aprobar" una sesion reutiliza la misma
/// transicion de estado que "publicar" (no hay un paso de revision separado en el modelo actual).
/// </summary>
public sealed class EventSessionOpsService : IEventSessionOpsService
{
    private readonly TechRidersDbContext _dbContext;

    public EventSessionOpsService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SetEventStatusAsync(Guid eventId, string statusName, CancellationToken cancellationToken = default) =>
        SetStatusAsync(statusName, "Event", async status =>
        {
            var evento = await _dbContext.Set<Event>().FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken)
                ?? throw new InvalidOperationException("Evento no encontrado.");
            evento.StatusId = status.Id;
            evento.UpdatedAt = DateTime.UtcNow;
        }, cancellationToken);

    public Task SetSessionStatusAsync(Guid sessionId, string statusName, CancellationToken cancellationToken = default) =>
        SetStatusAsync(statusName, "Session", async status =>
        {
            var session = await _dbContext.Set<Session>().FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken)
                ?? throw new InvalidOperationException("Sesion no encontrada.");
            session.StatusId = status.Id;
            session.UpdatedAt = DateTime.UtcNow;
        }, cancellationToken);

    private async Task SetStatusAsync(string statusName, string scope, Func<Status, Task> apply, CancellationToken cancellationToken)
    {
        var status = await _dbContext.Set<Status>().FirstOrDefaultAsync(s => s.Name == statusName && s.Scope == scope, cancellationToken);
        if (status is null)
        {
            status = new Status { Id = Guid.NewGuid(), Name = statusName, Scope = scope };
            await _dbContext.Set<Status>().AddAsync(status, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        await apply(status);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddSpeakerAsync(Guid sessionId, Guid userId, bool isMainSpeaker, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Set<SessionSpeaker>().AnyAsync(s => s.SessionId == sessionId && s.UserId == userId, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("La persona ya esta asignada como ponente de esta sesion.");
        }

        await _dbContext.Set<SessionSpeaker>().AddAsync(new SessionSpeaker { SessionId = sessionId, UserId = userId, IsMainSpeaker = isMainSpeaker }, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveSpeakerAsync(Guid sessionId, Guid userId, CancellationToken cancellationToken = default)
    {
        var speaker = await _dbContext.Set<SessionSpeaker>().FirstOrDefaultAsync(s => s.SessionId == sessionId && s.UserId == userId, cancellationToken)
            ?? throw new InvalidOperationException("Ponente no encontrado en esta sesion.");

        _dbContext.Set<SessionSpeaker>().Remove(speaker);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SyncSkillsAsync(Guid sessionId, IReadOnlyCollection<Guid> skillIds, CancellationToken cancellationToken = default)
    {
        var current = await _dbContext.Set<SessionSkill>().Where(s => s.SessionId == sessionId).ToListAsync(cancellationToken);
        _dbContext.Set<SessionSkill>().RemoveRange(current.Where(c => !skillIds.Contains(c.SkillId)));

        var existingIds = current.Select(c => c.SkillId).ToHashSet();
        foreach (var skillId in skillIds.Where(id => !existingIds.Contains(id)))
        {
            await _dbContext.Set<SessionSkill>().AddAsync(new SessionSkill { SessionId = sessionId, SkillId = skillId }, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<List<EventRegistration>> GetEventRegistrationsAsync(Guid eventId, CancellationToken cancellationToken = default) =>
        _dbContext.Set<EventRegistration>().Include(r => r.User).Where(r => r.EventId == eventId).ToListAsync(cancellationToken);

    public async Task UpdateEventRegistrationStatusAsync(Guid registrationId, string status, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<RegistrationStatus>(status, ignoreCase: true, out var parsedStatus))
        {
            throw new ArgumentException($"Estado de inscripcion '{status}' no reconocido.");
        }

        var registration = await _dbContext.Set<EventRegistration>().FirstOrDefaultAsync(r => r.Id == registrationId, cancellationToken)
            ?? throw new InvalidOperationException("Inscripcion no encontrada.");

        registration.RegistrationStatus = parsedStatus;
        registration.Attended = parsedStatus == RegistrationStatus.Attended;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
