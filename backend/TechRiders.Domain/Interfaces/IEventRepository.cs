using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de Eventos
/// Extiende las operaciones básicas con consultas específicas del dominio
/// </summary>
public interface IEventRepository : IRepository<Event>
{
    /// <summary>
    /// Obtiene un evento incluyendo sus sesiones
    /// </summary>
    Task<Event?> GetEventWithSessionsAsync(Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene eventos activos en un rango de fechas
    /// </summary>
    Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene eventos activos (no eliminados lógicamente)
    /// </summary>
    Task<IEnumerable<Event>> GetActiveEventsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene eventos próximos (posteriores a la fecha actual)
    /// </summary>
    Task<IEnumerable<Event>> GetUpcomingEventsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca eventos por nombre o descripción
    /// </summary>
    Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm, CancellationToken cancellationToken = default);
}
