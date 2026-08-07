namespace TechRiders.Application.DTOs.Requests;

/// <summary>
/// Request to update an existing tutorial
/// </summary>
public class UpdateTutorialRequest
{
    /// <summary>
    /// Tutorial ID to update
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Updated tutorial title
    /// </summary>
    public string? Titulo { get; set; }

    /// <summary>
    /// Updated abstract
    /// </summary>
    public string? Extracto { get; set; }

    /// <summary>
    /// Updated categories JSON
    /// </summary>
    public string? CategoriasJson { get; set; }

    /// <summary>
    /// Updated external URL
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Row version for optimistic concurrency
    /// </summary>
    public required byte[] RowVersion { get; set; }
}
