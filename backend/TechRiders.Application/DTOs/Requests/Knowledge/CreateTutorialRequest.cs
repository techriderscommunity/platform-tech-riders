namespace TechRiders.Application.DTOs.Requests.Knowledge;

/// <summary>
/// Request to create a new tutorial
/// </summary>
public class CreateTutorialRequest
{
    /// <summary>
    /// Tutorial unique slug for URLs
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// Tutorial title
    /// </summary>
    public required string Titulo { get; set; }

    /// <summary>
    /// Short summary or abstract
    /// </summary>
    public required string Extracto { get; set; }

    /// <summary>
    /// Tutorial author name
    /// </summary>
    public required string Autor { get; set; }

    /// <summary>
    /// Categories as JSON array string
    /// </summary>
    public required string CategoriasJson { get; set; }

    /// <summary>
    /// External URL to full tutorial
    /// </summary>
    public required string Url { get; set; }
}
