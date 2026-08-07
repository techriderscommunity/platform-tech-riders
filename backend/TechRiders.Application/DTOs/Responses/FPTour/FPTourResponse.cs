namespace TechRiders.Application.DTOs.Responses.FPTour;

/// <summary>
/// DTO de respuesta para un tour FP
/// </summary>
public class FPTourResponse
{
    /// <summary>
    /// Identificador del tour
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID del centro educativo
    /// </summary>
    public Guid CenterId { get; set; }

    /// <summary>
    /// Nombre del centro
    /// </summary>
    public string CenterName { get; set; } = string.Empty;

    /// <summary>
    /// ID del ambassador asignado
    /// </summary>
    public Guid AmbassadorId { get; set; }

    /// <summary>
    /// Nombre completo del ambassador
    /// </summary>
    public string AmbassadorName { get; set; } = string.Empty;

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
    /// Fecha de creación del registro
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha de la última actualización
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Indica si el registro está activo
    /// </summary>
    public bool IsActive { get; set; }
}
