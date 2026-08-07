using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.Center;

/// <summary>
/// DTO para crear un nuevo centro educativo
/// </summary>
public class CreateCenterRequest
{
    /// <summary>
    /// Nombre del centro
    /// </summary>
    /// <example>IES Juan de la Cierva</example>
    [Required(ErrorMessage = "El nombre del centro es obligatorio")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Persona de contacto en el centro
    /// </summary>
    /// <example>María Rodríguez</example>
    [StringLength(200, ErrorMessage = "La persona de contacto no puede exceder 200 caracteres")]
    public string? ContactPerson { get; set; }

    /// <summary>
    /// Email de contacto del centro
    /// </summary>
    /// <example>info@iescierva.edu</example>
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    [StringLength(200, ErrorMessage = "El email no puede exceder 200 caracteres")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono de contacto
    /// </summary>
    /// <example>+34911234567</example>
    [Phone(ErrorMessage = "El teléfono no es válido")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string? Phone { get; set; }

    /// <summary>
    /// Localidad del centro
    /// </summary>
    /// <example>Madrid</example>
    [StringLength(200, ErrorMessage = "La localidad no puede exceder 200 caracteres")]
    public string? Locality { get; set; }

    /// <summary>
    /// Estudios que ofrece el centro
    /// </summary>
    /// <example>FP Desarrollo de Aplicaciones Web, FP Administración de Sistemas</example>
    [StringLength(1000, ErrorMessage = "Los estudios no pueden exceder 1000 caracteres")]
    public string? Studies { get; set; }

    /// <summary>
    /// Especialidad del centro
    /// </summary>
    /// <example>Tecnología e Informática</example>
    [StringLength(500, ErrorMessage = "La especialidad no puede exceder 500 caracteres")]
    public string? Specialty { get; set; }

    /// <summary>
    /// Número aproximado de estudiantes
    /// </summary>
    /// <example>450</example>
    [Range(0, 100000, ErrorMessage = "El número de estudiantes debe estar entre 0 y 100000")]
    public int? NumberStudents { get; set; }

    /// <summary>
    /// Ubicación física del centro
    /// </summary>
    /// <example>Calle Principal 123, 28001 Madrid</example>
    [StringLength(500, ErrorMessage = "La ubicación no puede exceder 500 caracteres")]
    public string? Location { get; set; }

    /// <summary>
    /// Información sobre parking disponible
    /// </summary>
    /// <example>Parking gratuito para visitantes</example>
    [StringLength(500, ErrorMessage = "La información de parking no puede exceder 500 caracteres")]
    public string? Parking { get; set; }

    /// <summary>
    /// URL de perfil de LinkedIn
    /// </summary>
    /// <example>https://linkedin.com/school/iescierva</example>
    [Url(ErrorMessage = "La URL de LinkedIn no es válida")]
    [StringLength(300, ErrorMessage = "La URL no puede exceder 300 caracteres")]
    public string? LinkedIn { get; set; }

    /// <summary>
    /// Usuario o URL de Instagram
    /// </summary>
    /// <example>@iescierva</example>
    [StringLength(300, ErrorMessage = "Instagram no puede exceder 300 caracteres")]
    public string? Instagram { get; set; }
}
