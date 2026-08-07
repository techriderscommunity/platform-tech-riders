namespace TechRiders.Application.DTOs.Responses;

/// <summary>
/// Response model for a job offer
/// </summary>
public class OfertaResponse
{
    /// <summary>
    /// Offer unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Job title
    /// </summary>
    public required string Titulo { get; set; }

    /// <summary>
    /// Company name
    /// </summary>
    public required string Empresa { get; set; }

    /// <summary>
    /// Job description
    /// </summary>
    public required string Descripcion { get; set; }

    /// <summary>
    /// Salary amount
    /// </summary>
    public decimal Salario { get; set; }

    /// <summary>
    /// Job location
    /// </summary>
    public required string Ubicacion { get; set; }

    /// <summary>
    /// Work modality (0=Remote, 1=Hybrid, 2=On-site)
    /// </summary>
    public int Modalidad { get; set; }

    /// <summary>
    /// Job requirements
    /// </summary>
    public required string Requisitos { get; set; }

    /// <summary>
    /// Offer state (0=Draft, 1=Active, 2=Closed)
    /// </summary>
    public int Estado { get; set; }

    /// <summary>
    /// Publication date
    /// </summary>
    public DateTime FechaPublicacion { get; set; }

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
