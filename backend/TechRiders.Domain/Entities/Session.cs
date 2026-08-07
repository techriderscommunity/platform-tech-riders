using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechRiders.Domain.Entities;

/// <summary>
/// Representa una sesión dentro de un evento de TechRiders
/// </summary>
public class Session : BaseEntity
{
    /// <summary>
    /// Título de la sesión
    /// </summary>
    [Required(ErrorMessage = "El título de la sesión es obligatorio")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la sesión
    /// </summary>
    [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Hora de inicio de la sesión
    /// </summary>
    [Required(ErrorMessage = "La hora de inicio es obligatoria")]
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Hora de finalización de la sesión
    /// </summary>
    [Required(ErrorMessage = "La hora de finalización es obligatoria")]
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Nombre del ponente o speaker
    /// </summary>
    [StringLength(150, ErrorMessage = "El nombre del ponente no puede exceder 150 caracteres")]
    public string? Speaker { get; set; }

    /// <summary>
    /// Sala o ubicación específica de la sesión
    /// </summary>
    [StringLength(100, ErrorMessage = "El nombre de la sala no puede exceder 100 caracteres")]
    public string? Room { get; set; }

    /// <summary>
    /// Nivel de dificultad de la sesión (Básico, Intermedio, Avanzado)
    /// </summary>
    [StringLength(50, ErrorMessage = "El nivel no puede exceder 50 caracteres")]
    public string? Level { get; set; }

    /// <summary>
    /// Capacidad máxima de asistentes para esta sesión
    /// </summary>
    [Range(1, 1000, ErrorMessage = "La capacidad debe estar entre 1 y 1000")]
    public int? MaxCapacity { get; set; }

    /// <summary>
    /// ID del evento al que pertenece esta sesión
    /// </summary>
    [Required(ErrorMessage = "El ID del evento es obligatorio")]
    public Guid EventId { get; set; }

    /// <summary>
    /// Navegación al evento padre
    /// </summary>
    [ForeignKey(nameof(EventId))]
    public virtual Event Event { get; set; } = null!;

    /// <summary>
    /// Valida que la hora de finalización sea posterior a la hora de inicio
    /// </summary>
    public bool ValidateTimes()
    {
        return EndTime > StartTime;
    }

    /// <summary>
    /// Calcula la duración de la sesión en minutos
    /// </summary>
    public int GetDurationInMinutes()
    {
        return (int)(EndTime - StartTime).TotalMinutes;
    }
}
