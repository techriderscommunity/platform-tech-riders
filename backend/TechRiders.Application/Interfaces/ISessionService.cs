using TechRiders.Application.DTOs.Requests.Session;
using TechRiders.Application.DTOs.Responses.Sessions;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de sesiones
/// Define el contrato de operaciones de negocio para sesiones
/// </summary>
public interface ISessionService
{
    /// <summary>
    /// Obtiene todas las sesiones activas
    /// </summary>
    Task<IEnumerable<SessionResponse>> GetAllSessionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una sesión por su ID
    /// </summary>
    Task<SessionResponse?> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las sesiones de un evento específico
    /// </summary>
    Task<IEnumerable<SessionResponse>> GetSessionsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene sesiones por ponente
    /// </summary>
    Task<IEnumerable<SessionResponse>> GetSessionsBySpeakerAsync(string speaker, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene sesiones por nivel de dificultad
    /// </summary>
    Task<IEnumerable<SessionResponse>> GetSessionsByLevelAsync(string level, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea una nueva sesión
    /// </summary>
    Task<SessionResponse> CreateSessionAsync(CreateSessionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza una sesión existente
    /// </summary>
    Task<SessionResponse?> UpdateSessionAsync(Guid id, UpdateSessionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina lógicamente una sesión
    /// </summary>
    Task<bool> DeleteSessionAsync(Guid id, CancellationToken cancellationToken = default);
}
