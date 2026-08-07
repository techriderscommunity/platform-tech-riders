using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.Ambassador;

/// <summary>
/// DTO para actualizar un ambassador existente
/// </summary>
public class UpdateAmbassadorRequest
{
    /// <summary>
    /// Apodo o nickname del ambassador
    /// </summary>
    [StringLength(100, ErrorMessage = "El nickname no puede exceder 100 caracteres")]
    public string? Nickname { get; set; }

    /// <summary>
    /// Nombre del ambassador
    /// </summary>
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string? Name { get; set; }

    /// <summary>
    /// Apellido del ambassador
    /// </summary>
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
    public string? LastName { get; set; }

    /// <summary>
    /// Email de contacto
    /// </summary>
    [EmailAddress(ErrorMessage = "El email no es válido")]
    [StringLength(200, ErrorMessage = "El email no puede exceder 200 caracteres")]
    public string? Email { get; set; }

    /// <summary>
    /// Teléfono de contacto
    /// </summary>
    [Phone(ErrorMessage = "El teléfono no es válido")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string? Phone { get; set; }

    /// <summary>
    /// Localidad del ambassador
    /// </summary>
    [StringLength(200, ErrorMessage = "La localidad no puede exceder 200 caracteres")]
    public string? Locality { get; set; }

    /// <summary>
    /// Indica si el ambassador está actualmente trabajando
    /// </summary>
    public bool? IsWorking { get; set; }

    /// <summary>
    /// ID de la categoría principal
    /// </summary>
    public int? CategoryId { get; set; }

    /// <summary>
    /// Otra categoría personalizada
    /// </summary>
    [StringLength(200, ErrorMessage = "La categoría no puede exceder 200 caracteres")]
    public string? OtherCategory { get; set; }

    /// <summary>
    /// Descripción sobre el ambassador
    /// </summary>
    [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
    public string? About { get; set; }

    /// <summary>
    /// Habilidades del ambassador
    /// </summary>
    [StringLength(1000, ErrorMessage = "Las habilidades no pueden exceder 1000 caracteres")]
    public string? Skill { get; set; }

    /// <summary>
    /// URL de perfil de LinkedIn
    /// </summary>
    [Url(ErrorMessage = "La URL de LinkedIn no es válida")]
    [StringLength(300, ErrorMessage = "La URL no puede exceder 300 caracteres")]
    public string? LinkedIn { get; set; }

    /// <summary>
    /// Usuario o URL de Instagram
    /// </summary>
    [StringLength(300, ErrorMessage = "Instagram no puede exceder 300 caracteres")]
    public string? Instagram { get; set; }

    /// <summary>
    /// Usuario o URL de GitHub
    /// </summary>
    [StringLength(300, ErrorMessage = "GitHub no puede exceder 300 caracteres")]
    public string? Github { get; set; }
}
