using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.Event;

/// <summary>
/// DTO para crear un nuevo evento
/// </summary>
public class CreateEventRequest
{
    /// <summary>
    /// Nombre del evento
    /// </summary>
    /// <example>TechRiders Summit 2026</example>
    [Required(ErrorMessage = "El nombre del evento es obligatorio")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del evento
    /// </summary>
    /// <example>Evento anual de tecnología con las últimas tendencias</example>
    [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Fecha de inicio del evento
    /// </summary>
    /// <example>2026-10-15T09:00:00Z</example>
    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Fecha de finalización del evento
    /// </summary>
    /// <example>2026-10-17T18:00:00Z</example>
    [Required(ErrorMessage = "La fecha de finalización es obligatoria")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Ubicación del evento
    /// </summary>
    /// <example>Centro de Convenciones, Madrid</example>
    [StringLength(300, ErrorMessage = "La ubicación no puede exceder 300 caracteres")]
    public string? Location { get; set; }

    /// <summary>
    /// Capacidad máxima de asistentes
    /// </summary>
    /// <example>500</example>
    [Range(1, 10000, ErrorMessage = "La capacidad debe estar entre 1 y 10000")]
    public int? MaxCapacity { get; set; }
}
