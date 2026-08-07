namespace TechRiders.Domain.Entities;

/// <summary>
/// Entidad base con propiedades comunes para todas las entidades del dominio
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único de la entidad
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Fecha y hora de creación del registro
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha y hora de la última modificación del registro
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Indica si el registro está activo o ha sido eliminado lógicamente
    /// </summary>
    public bool IsActive { get; set; } = true;

    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
