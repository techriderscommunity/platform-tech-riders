using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.FPTour;

/// <summary>
/// DTO para crear un nuevo tour FP
/// </summary>
public class CreateFPTourRequest
{
    /// <summary>
    /// ID del centro educativo
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required(ErrorMessage = "El centro es obligatorio")]
    public Guid CenterId { get; set; }

    /// <summary>
    /// ID del ambassador asignado
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required(ErrorMessage = "El ambassador es obligatorio")]
    public Guid AmbassadorId { get; set; }

    /// <summary>
    /// Indica si se ha contactado con el centro
    /// </summary>
    /// <example>false</example>
    public bool HasContactCenter { get; set; }

    /// <summary>
    /// Indica si se ha contactado con el ambassador
    /// </summary>
    /// <example>false</example>
    public bool HasContactAmbassador { get; set; }

    /// <summary>
    /// Indica si se ha agendado una fecha
    /// </summary>
    /// <example>false</example>
    public bool HasScheduledDate { get; set; }

    /// <summary>
    /// Indica si se ha recibido feedback del centro
    /// </summary>
    /// <example>false</example>
    public bool HasFeedbackCenter { get; set; }

    /// <summary>
    /// Indica si se ha recibido feedback del ambassador
    /// </summary>
    /// <example>false</example>
    public bool HasFeedbackAmbassador { get; set; }

    /// <summary>
    /// Indica si el centro ha enviado fotos
    /// </summary>
    /// <example>false</example>
    public bool HasPhotosCenter { get; set; }

    /// <summary>
    /// Indica si el ambassador ha enviado fotos
    /// </summary>
    /// <example>false</example>
    public bool HasPhotosAmbassador { get; set; }

    /// <summary>
    /// Indica si se ha entregado algo al centro
    /// </summary>
    /// <example>false</example>
    public bool HasDeliveredCenter { get; set; }

    /// <summary>
    /// Indica si se ha entregado algo al ambassador
    /// </summary>
    /// <example>false</example>
    public bool HasDeliveredAmbassador { get; set; }
}
