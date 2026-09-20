using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Operaciones administrativas de Eventos/Sesiones que complementan los CRUD ya existentes
/// (IEventService/ISessionService): publicar/cancelar (via catalogo Status generico), asignar
/// ponentes, sincronizar skills y gestionar inscripciones.
/// </summary>
public interface IEventSessionOpsService
{
    Task SetEventStatusAsync(Guid eventId, string statusName, CancellationToken cancellationToken = default);
    Task SetSessionStatusAsync(Guid sessionId, string statusName, CancellationToken cancellationToken = default);
    Task AddSpeakerAsync(Guid sessionId, Guid userId, bool isMainSpeaker, CancellationToken cancellationToken = default);
    Task RemoveSpeakerAsync(Guid sessionId, Guid userId, CancellationToken cancellationToken = default);
    Task SyncSkillsAsync(Guid sessionId, IReadOnlyCollection<Guid> skillIds, CancellationToken cancellationToken = default);
    Task<List<EventRegistration>> GetEventRegistrationsAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task UpdateEventRegistrationStatusAsync(Guid registrationId, string status, CancellationToken cancellationToken = default);
}

public static class EventSessionStatusNames
{
    public const string Published = "Publicado";
    public const string Cancelled = "Cancelado";
}
