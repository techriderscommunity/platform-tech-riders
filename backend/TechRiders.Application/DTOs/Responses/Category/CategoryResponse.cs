namespace TechRiders.Application.DTOs.Responses.Category;

/// <summary>
/// DTO de respuesta para una categoría
/// </summary>
public class CategoryResponse
{
    /// <summary>
    /// Identificador de la categoría
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre de la categoría
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la categoría padre (null si es categoría principal)
    /// </summary>
    public int? FatherId { get; set; }

    /// <summary>
    /// Nombre de la categoría padre
    /// </summary>
    public string? FatherName { get; set; }

    /// <summary>
    /// Indica si la categoría está activa
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Lista de subcategorías
    /// </summary>
    public List<CategoryResponse>? SubCategories { get; set; }
}
