using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.FPTour;

/// <summary>
/// DTO para actualizar un tour FP existente
/// </summary>
public class UpdateFPTourRequest
{
    /// <summary>
    /// ID del centro educativo
    /// </summary>
    public Guid? CenterId { get; set; }

    /// <summary>
    /// ID del ambassador asignado
    /// </summary>
    public Guid? AmbassadorId { get; set; }

    /// <summary>
    /// Indica si se ha contactado con el centro
    /// </summary>
    public bool? HasContactCenter { get; set; }

    /// <summary>
    /// Indica si se ha contactado con el ambassador
    /// </summary>
    public bool? HasContactAmbassador { get; set; }

    /// <summary>
    /// Indica si se ha agendado una fecha
    /// </summary>
    public bool? HasScheduledDate { get; set; }

    /// <summary>
    /// Indica si se ha recibido feedback del centro
    /// </summary>
    public bool? HasFeedbackCenter { get; set; }

    /// <summary>
    /// Indica si se ha recibido feedback del ambassador
    /// </summary>
    public bool? HasFeedbackAmbassador { get; set; }

    /// <summary>
    /// Indica si el centro ha enviado fotos
    /// </summary>
    public bool? HasPhotosCenter { get; set; }

    /// <summary>
    /// Indica si el ambassador ha enviado fotos
    /// </summary>
    public bool? HasPhotosAmbassador { get; set; }

    /// <summary>
    /// Indica si se ha entregado algo al centro
    /// </summary>
    public bool? HasDeliveredCenter { get; set; }

    /// <summary>
    /// Indica si se ha entregado algo al ambassador
    /// </summary>
    public bool? HasDeliveredAmbassador { get; set; }
}
