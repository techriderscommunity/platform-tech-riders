namespace TechRiders.Application.DTOs.Responses;

/// <summary>
/// Response model for a job application
/// </summary>
public class CandidaturaResponse
{
    /// <summary>
    /// Application unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Job offer ID
    /// </summary>
    public Guid OfertaId { get; set; }

    /// <summary>
    /// Junior candidate ID
    /// </summary>
    public required string JuniorId { get; set; }

    /// <summary>
    /// Candidate full name
    /// </summary>
    public required string NombreJunior { get; set; }

    /// <summary>
    /// Candidate email
    /// </summary>
    public required string EmailJunior { get; set; }

    /// <summary>
    /// Application state (0=Pending, 1=Interview, 2=Rejected, 3=Hired)
    /// </summary>
    public int Estado { get; set; }

    /// <summary>
    /// Application submission date
    /// </summary>
    public DateTime FechaSolicitud { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Soft delete flag
    /// </summary>
    public bool IsActive { get; set; }
}
