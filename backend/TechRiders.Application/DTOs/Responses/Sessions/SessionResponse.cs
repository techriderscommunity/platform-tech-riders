using TechRiders.Application.DTOs.Responses.Event;
namespace TechRiders.Application.DTOs.Responses.Sessions;
/// <summary>
/// DTO de respuesta para una sesión
/// </summary>
public class SessionResponse
{
    /// <summary>
    /// Identificador de la sesión
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Título de la sesión 
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la sesión
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Hora de inicio de la sesión
    /// </summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Hora de finalización de la sesión
    /// </summary>
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Nombre del ponente
    /// </summary>
    public string? Speaker { get; set; }

    /// <summary>
    /// Sala de la sesión
    /// </summary>
    public string? Room { get; set; }

    /// <summary>
    /// Nivel de dificultad
    /// </summary>
    public string? Level { get; set; }

    /// <summary>
    /// Capacidad máxima de asistentes
    /// </summary>
    public int? MaxCapacity { get; set; }

    /// <summary>
    /// ID del evento al que pertenece
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha de la última actualización
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Indica si la sesión está activa
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Información básica del evento (opcional, solo cuando se incluye)
    /// </summary>
    public EventBasicResponse? Event { get; set; }
}


