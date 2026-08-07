namespace TechRiders.Application.DTOs.Responses;

/// <summary>
/// Response model for a tutorial
/// </summary>
public class TutorialResponse
{
    /// <summary>
    /// Tutorial unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Tutorial slug for URLs
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// Tutorial title
    /// </summary>
    public required string Titulo { get; set; }

    /// <summary>
    /// Tutorial abstract
    /// </summary>
    public required string Extracto { get; set; }

    /// <summary>
    /// Author name
    /// </summary>
    public required string Autor { get; set; }

    /// <summary>
    /// Publication date
    /// </summary>
    public DateTime FechaPublicacion { get; set; }

    /// <summary>
    /// Categories JSON
    /// </summary>
    public required string CategoriasJson { get; set; }

    /// <summary>
    /// External tutorial URL
    /// </summary>
    public required string Url { get; set; }

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
