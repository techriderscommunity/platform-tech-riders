using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de Sesiones
/// Extiende las operaciones básicas con consultas específicas del dominio
/// </summary>
public interface ISessionRepository : IRepository<Session>
{
    /// <summary>
    /// Obtiene una sesión incluyendo su evento padre
    /// </summary>
    Task<Session?> GetSessionWithEventAsync(Guid sessionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las sesiones de un evento específico
    /// </summary>
    Task<IEnumerable<Session>> GetSessionsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene sesiones por ponente
    /// </summary>
    Task<IEnumerable<Session>> GetSessionsBySpeakerAsync(string speaker, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene sesiones por nivel de dificultad
    /// </summary>
    Task<IEnumerable<Session>> GetSessionsByLevelAsync(string level, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene sesiones activas (no eliminadas lógicamente)
    /// </summary>
    Task<IEnumerable<Session>> GetActiveSessionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si hay conflictos de horario para una sala en un evento
    /// </summary>
    Task<bool> HasTimeConflictAsync(Guid eventId, string room, TimeSpan startTime, TimeSpan endTime, Guid? excludeSessionId   = null, CancellationToken cancellationToken = default);
}
