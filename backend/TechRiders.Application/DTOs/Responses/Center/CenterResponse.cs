namespace TechRiders.Application.DTOs.Responses.Center;

/// <summary>
/// DTO de respuesta para un centro educativo
/// </summary>
public class CenterResponse
{
    /// <summary>
    /// Identificador del centro
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre del centro
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Persona de contacto en el centro
    /// </summary>
    public string? ContactPerson { get; set; }

    /// <summary>
    /// Email de contacto del centro
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono de contacto
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Localidad del centro
    /// </summary>
    public string? Locality { get; set; }

    /// <summary>
    /// Estudios que ofrece el centro
    /// </summary>
    public string? Studies { get; set; }

    /// <summary>
    /// Especialidad del centro
    /// </summary>
    public string? Specialty { get; set; }

    /// <summary>
    /// Número aproximado de estudiantes
    /// </summary>
    public int? NumberStudents { get; set; }

    /// <summary>
    /// Ubicación física del centro
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Información sobre parking disponible
    /// </summary>
    public string? Parking { get; set; }

    /// <summary>
    /// URL de perfil de LinkedIn
    /// </summary>
    public string? LinkedIn { get; set; }

    /// <summary>
    /// Usuario o URL de Instagram
    /// </summary>
    public string? Instagram { get; set; }

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
