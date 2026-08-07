using TechRiders.Application.DTOs.Requests.Event;
using TechRiders.Application.DTOs.Responses.Event;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de eventos
/// Define el contrato de operaciones de negocio para eventos
/// </summary>
public interface IEventService
{
    /// <summary>
    /// Obtiene todos los eventos activos
    /// </summary>
    Task<IEnumerable<EventResponse>> GetAllEventsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un evento por su ID incluyendo sus sesiones
    /// </summary>
    Task<EventResponse?> GetEventByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene eventos próximos (futuros)
    /// </summary>
    Task<IEnumerable<EventResponse>> GetUpcomingEventsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca eventos por término de búsqueda
    /// </summary>
    Task<IEnumerable<EventResponse>> SearchEventsAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene eventos en un rango de fechas
    /// </summary>
    Task<IEnumerable<EventResponse>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea un nuevo evento
    /// </summary>
    Task<EventResponse> CreateEventAsync(CreateEventRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza un evento existente
    /// </summary>
    Task<EventResponse?> UpdateEventAsync(Guid id, UpdateEventRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina lógicamente un evento
    /// </summary>
    Task<bool> DeleteEventAsync(Guid id, CancellationToken cancellationToken = default);
}
