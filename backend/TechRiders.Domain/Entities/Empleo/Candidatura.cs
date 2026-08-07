using System.ComponentModel.DataAnnotations;

namespace TechRiders.Domain.Entities.Empleo;

/// <summary>
/// Representa una candidatura de un junior a una oferta de empleo
/// </summary>
public class Candidatura : BaseEntity
{
    /// <summary>
    /// ID de la oferta a la que se candida
    /// </summary>
    public Guid OfertaId { get; set; }

    /// <summary>
    /// ID del junior que se candida
    /// </summary>
    [Required]
    [StringLength(255)]
    public string JuniorId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del junior
    /// </summary>
    [Required]
    [StringLength(255)]
    public string NombreJunior { get; set; } = string.Empty;

    /// <summary>
    /// Email del junior
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string EmailJunior { get; set; } = string.Empty;

    /// <summary>
    /// Estado actual de la candidatura
    /// </summary>
    public CandidaturaEstado Estado { get; set; }

    /// <summary>
    /// Fecha de solicitud
    /// </summary>
    public DateTime FechaSolicitud { get; set; }

    /// <summary>
    /// Token para concurrencia optimista
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    /// <summary>
    /// Relación con la oferta (lazy-loaded)
    /// </summary>
    public virtual Oferta? Oferta { get; set; }

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    protected Candidatura()
    {
    }

    /// <summary>
    /// Factory method para crear una nueva candidatura
    /// </summary>
    public static Candidatura Create(Guid ofertaId, string juniorId, string nombreJunior, string emailJunior)
    {
        return new Candidatura
        {
            OfertaId = ofertaId,
            JuniorId = juniorId,
            NombreJunior = nombreJunior,
            EmailJunior = emailJunior,
            Estado = CandidaturaEstado.Pendiente,
            FechaSolicitud = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Avanza la candidatura a entrevista
    /// </summary>
    public void AvanzarAEntrevista()
    {
        Estado = CandidaturaEstado.Entrevista;
    }

    /// <summary>
    /// Rechaza la candidatura
    /// </summary>
    public void Rechazar()
    {
        Estado = CandidaturaEstado.Rechazado;
    }

    /// <summary>
    /// Contrata al candidato
    /// </summary>
    public void Contratar()
    {
        Estado = CandidaturaEstado.Contratado;
    }
}
