namespace TechRiders.Application.DTOs.Requests;

/// <summary>
/// Request to update an existing job offer
/// </summary>
public class UpdateOfertaRequest
{
    /// <summary>
    /// Offer ID to update
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Updated job title
    /// </summary>
    public string? Titulo { get; set; }

    /// <summary>
    /// Updated company name
    /// </summary>
    public string? Empresa { get; set; }

    /// <summary>
    /// Updated description
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Updated salary
    /// </summary>
    public decimal? Salario { get; set; }

    /// <summary>
    /// Updated location
    /// </summary>
    public string? Ubicacion { get; set; }

    /// <summary>
    /// Updated modality
    /// </summary>
    public int? Modalidad { get; set; }

    /// <summary>
    /// Updated requirements
    /// </summary>
    public string? Requisitos { get; set; }

    /// <summary>
    /// Updated state (0=Draft, 1=Active, 2=Closed)
    /// </summary>
    public int? Estado { get; set; }

    /// <summary>
    /// Row version for optimistic concurrency
    /// </summary>
    public required byte[] RowVersion { get; set; }
}
