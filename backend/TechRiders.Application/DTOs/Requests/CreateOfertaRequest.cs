namespace TechRiders.Application.DTOs.Requests;

/// <summary>
/// Request to create a new job offer
/// </summary>
public class CreateOfertaRequest
{
    /// <summary>
    /// Job title
    /// </summary>
    public required string Titulo { get; set; }

    /// <summary>
    /// Company name
    /// </summary>
    public required string Empresa { get; set; }

    /// <summary>
    /// Job description/details
    /// </summary>
    public required string Descripcion { get; set; }

    /// <summary>
    /// Salary range or amount
    /// </summary>
    public decimal Salario { get; set; }

    /// <summary>
    /// Job location
    /// </summary>
    public required string Ubicacion { get; set; }

    /// <summary>
    /// Work modality (Remote, Hybrid, On-site)
    /// </summary>
    public required int Modalidad { get; set; }

    /// <summary>
    /// Job requirements
    /// </summary>
    public required string Requisitos { get; set; }
}
