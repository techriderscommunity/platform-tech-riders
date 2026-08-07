using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.Session;

/// <summary>
/// DTO para actualizar una sesión existente
/// </summary>
public class UpdateSessionRequest
{
    /// <summary>
    /// Título de la sesión
    /// </summary>
    /// <example>Introducción avanzada a .NET 10</example>
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres")]
    public string? Title { get; set; }

    /// <summary>
    /// Descripción de la sesión
    /// </summary>
    /// <example>Descubre las nuevas características avanzadas de .NET 10</example>
    [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Hora de inicio de la sesión
    /// </summary>
    /// <example>10:00:00</example>
    public TimeSpan? StartTime { get; set; }

    /// <summary>
    /// Hora de finalización de la sesión
    /// </summary>
    /// <example>11:30:00</example>
    public TimeSpan? EndTime { get; set; }

    /// <summary>
    /// Nombre del ponente o speaker
    /// </summary>
    /// <example>Juan López</example>
    [StringLength(150, ErrorMessage = "El nombre del ponente no puede exceder 150 caracteres")]
    public string? Speaker { get; set; }

    /// <summary>
    /// Sala o ubicación específica de la sesión
    /// </summary>
    /// <example>Sala B</example>
    [StringLength(100, ErrorMessage = "El nombre de la sala no puede exceder 100 caracteres")]
    public string? Room { get; set; }

    /// <summary>
    /// Nivel de dificultad de la sesión
    /// </summary>
    /// <example>Avanzado</example>
    [StringLength(50, ErrorMessage = "El nivel no puede exceder 50 caracteres")]
    public string? Level { get; set; }

    /// <summary>
    /// Capacidad máxima de asistentes para esta sesión
    /// </summary>
    /// <example>30</example>
    [Range(1, 1000, ErrorMessage = "La capacidad debe estar entre 1 y 1000")]
    public int? MaxCapacity { get; set; }
}
