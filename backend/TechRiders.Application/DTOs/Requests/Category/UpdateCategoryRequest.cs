using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.Category;

/// <summary>
/// DTO para actualizar una categoría existente
/// </summary>
public class UpdateCategoryRequest
{
    /// <summary>
    /// Nombre de la categoría
    /// </summary>
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string? Name { get; set; }

    /// <summary>
    /// Identificador de la categoría padre (null si es categoría principal)
    /// </summary>
    public int? FatherId { get; set; }

    /// <summary>
    /// Indica si la categoría está activa
    /// </summary>
    public bool? Active { get; set; }
}
