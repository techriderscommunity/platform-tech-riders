using TechRiders.Application.DTOs.Responses.Sessions;

namespace TechRiders.Application.DTOs.Responses.Event;

/// <summary>
/// DTO de respuesta para un evento
/// </summary>
public class EventResponse
{
    /// <summary>
    /// Identificador del evento
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre del evento
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del evento
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Fecha de inicio del evento
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Fecha de finalización del evento
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Ubicación del evento
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Capacidad máxima de asistentes
    /// </summary>
    public int? MaxCapacity { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha de la última actualización
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Indica si el evento está activo
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Lista de sesiones del evento
    /// </summary>
    public List<SessionResponse>? Sessions { get; set; }
}
