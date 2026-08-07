using System.ComponentModel.DataAnnotations;

namespace TechRiders.Domain.Entities;

/// <summary>
/// Representa un tour de Formación Profesional
/// Relaciona un centro educativo con un ambassador
/// </summary>
public class FPTour : BaseEntity
{
    /// <summary>
    /// ID del centro educativo
    /// </summary>
    [Required(ErrorMessage = "El centro es obligatorio")]
    public Guid CenterId { get; set; }

    /// <summary>
    /// ID del ambassador asignado
    /// </summary>
    [Required(ErrorMessage = "El ambassador es obligatorio")]
    public Guid AmbassadorId { get; set; }

    /// <summary>
    /// Indica si se ha contactado con el centro
    /// </summary>
    public bool HasContactCenter { get; set; }

    /// <summary>
    /// Indica si se ha contactado con el ambassador
    /// </summary>
    public bool HasContactAmbassador { get; set; }

    /// <summary>
    /// Indica si se ha agendado una fecha
    /// </summary>
    public bool HasScheduledDate { get; set; }

    /// <summary>
    /// Indica si se ha recibido feedback del centro
    /// </summary>
    public bool HasFeedbackCenter { get; set; }

    /// <summary>
    /// Indica si se ha recibido feedback del ambassador
    /// </summary>
    public bool HasFeedbackAmbassador { get; set; }

    /// <summary>
    /// Indica si el centro ha enviado fotos
    /// </summary>
    public bool HasPhotosCenter { get; set; }

    /// <summary>
    /// Indica si el ambassador ha enviado fotos
    /// </summary>
    public bool HasPhotosAmbassador { get; set; }

    /// <summary>
    /// Indica si se ha entregado algo al centro
    /// </summary>
    public bool HasDeliveredCenter { get; set; }

    /// <summary>
    /// Indica si se ha entregado algo al ambassador
    /// </summary>
    public bool HasDeliveredAmbassador { get; set; }

    /// <summary>
    /// Centro asociado (navegación)
    /// </summary>
    public virtual Center Center { get; set; } = null!;

    /// <summary>
    /// Ambassador asignado (navegación)
    /// </summary>
    public virtual Ambassador Ambassador { get; set; } = null!;
}
