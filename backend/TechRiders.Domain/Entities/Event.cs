using System.ComponentModel.DataAnnotations;

namespace TechRiders.Domain.Entities;

/// <summary>
/// Representa un evento de TechRiders
/// </summary>
public class Event : BaseEntity
{
    /// <summary>
    /// Nombre del evento
    /// </summary>
    [Required(ErrorMessage = "El nombre del evento es obligatorio")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del evento
    /// </summary>
    [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Fecha de inicio del evento
    /// </summary>
    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Fecha de finalización del evento
    /// </summary>
    [Required(ErrorMessage = "La fecha de finalización es obligatoria")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Ubicación del evento
    /// </summary>
    [StringLength(300, ErrorMessage = "La ubicación no puede exceder 300 caracteres")]
    public string? Location { get; set; }

    /// <summary>
    /// Capacidad máxima de asistentes
    /// </summary>
    [Range(1, 10000, ErrorMessage = "La capacidad debe estar entre 1 y 10000")]
    public int? MaxCapacity { get; set; }

    /// <summary>
    /// Colección de sesiones asociadas al evento
    /// </summary>
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    /// <summary>
    /// Valida que la fecha de finalización sea posterior a la fecha de inicio
    /// </summary>
    public bool ValidateDates()
    {
        return EndDate > StartDate;
    }

    /// <summary>
    /// Calcula la duración del evento en días
    /// </summary>
    public int GetDurationInDays()
    {
        return (EndDate.Date - StartDate.Date).Days + 1;
    }
}
