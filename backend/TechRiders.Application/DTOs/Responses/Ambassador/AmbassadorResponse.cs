namespace TechRiders.Application.DTOs.Responses.Ambassador;

/// <summary>
/// DTO de respuesta para un ambassador
/// </summary>
public class AmbassadorResponse
{
    /// <summary>
    /// Identificador del ambassador
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Apodo o nickname del ambassador
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// Nombre del ambassador
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del ambassador
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Email de contacto
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono de contacto
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Localidad del ambassador
    /// </summary>
    public string? Locality { get; set; }

    /// <summary>
    /// Indica si el ambassador está actualmente trabajando
    /// </summary>
    public bool IsWorking { get; set; }

    /// <summary>
    /// ID de la categoría principal
    /// </summary>
    public int? CategoryId { get; set; }

    /// <summary>
    /// Nombre de la categoría
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Otra categoría personalizada
    /// </summary>
    public string? OtherCategory { get; set; }

    /// <summary>
    /// Descripción sobre el ambassador
    /// </summary>
    public string? About { get; set; }

    /// <summary>
    /// Habilidades del ambassador
    /// </summary>
    public string? Skill { get; set; }

    /// <summary>
    /// URL de perfil de LinkedIn
    /// </summary>
    public string? LinkedIn { get; set; }

    /// <summary>
    /// Usuario o URL de Instagram
    /// </summary>
    public string? Instagram { get; set; }

    /// <summary>
    /// Usuario o URL de GitHub
    /// </summary>
    public string? Github { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha de la última actualización
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Indica si el registro está activo
    /// </summary>
    public bool IsActive { get; set; }
}
