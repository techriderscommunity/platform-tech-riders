namespace TechRiders.Application.DTOs.Requests;

/// <summary>
/// Request to create a new job application
/// </summary>
public class CreateCandidaturaRequest
{
    /// <summary>
    /// Job offer ID
    /// </summary>
    public required Guid OfertaId { get; set; }

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
    /// Application cover letter or motivation
    /// </summary>
    public string? CartaPresentacion { get; set; }
}
