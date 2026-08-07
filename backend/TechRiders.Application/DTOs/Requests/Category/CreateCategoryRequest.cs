using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.Category;

/// <summary>
/// DTO para crear una nueva categoría
/// </summary>
public class CreateCategoryRequest
{
    /// <summary>
    /// Nombre de la categoría
    /// </summary>
    /// <example>Desarrollo y Programación Software</example>
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la categoría padre (null si es categoría principal)
    /// </summary>
    /// <example>null</example>
    public int? FatherId { get; set; }

    /// <summary>
    /// Indica si la categoría está activa
    /// </summary>
    /// <example>true</example>
    public bool Active { get; set; } = true;
}
