using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.Session;

/// <summary>
/// DTO para crear una nueva sesión
/// </summary>
public class CreateSessionRequest
{
    /// <summary>
    /// Título de la sesión
    /// </summary>
    /// <example>Introducción a .NET 10</example>
    [Required(ErrorMessage = "El título de la sesión es obligatorio")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la sesión
    /// </summary>
    /// <example>Descubre las nuevas características de .NET 10</example>
    [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Hora de inicio de la sesión
    /// </summary>
    /// <example>09:00:00</example>
    [Required(ErrorMessage = "La hora de inicio es obligatoria")]
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Hora de finalización de la sesión
    /// </summary>
    /// <example>10:30:00</example>
    [Required(ErrorMessage = "La hora de finalización es obligatoria")]
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Nombre del ponente o speaker
    /// </summary>
    /// <example>María García</example>
    [StringLength(150, ErrorMessage = "El nombre del ponente no puede exceder 150 caracteres")]
    public string? Speaker { get; set; }

    /// <summary>
    /// Sala o ubicación específica de la sesión
    /// </summary>
    /// <example>Sala A</example>
    [StringLength(100, ErrorMessage = "El nombre de la sala no puede exceder 100 caracteres")]
    public string? Room { get; set; }

    /// <summary>
    /// Nivel de dificultad de la sesión
    /// </summary>
    /// <example>Intermedio</example>
    [StringLength(50, ErrorMessage = "El nivel no puede exceder 50 caracteres")]
    public string? Level { get; set; }

    /// <summary>
    /// Capacidad máxima de asistentes para esta sesión
    /// </summary>
    /// <example>50</example>
    [Range(1, 1000, ErrorMessage = "La capacidad debe estar entre 1 y 1000")]
    public int? MaxCapacity { get; set; }

    /// <summary>
    /// ID del evento al que pertenece esta sesión
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "El ID del evento es obligatorio")]
    public Guid EventId { get; set; }
}
