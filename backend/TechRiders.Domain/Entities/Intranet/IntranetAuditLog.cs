using System.ComponentModel.DataAnnotations;

namespace TechRiders.Domain.Entities.Intranet;

/// <summary>
/// Log de auditoría para operaciones en la intranet
/// </summary>
public class IntranetAuditLog : BaseEntity
{
    /// <summary>
    /// Fecha y hora UTC de la operación
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// ID del usuario que realizó la acción (puede ser null para usuarios anónimos)
    /// </summary>
    public Guid? ActorUserId { get; set; }

    /// <summary>
    /// Email del usuario que realizó la acción
    /// </summary>
    [StringLength(255)]
    public string? ActorEmail { get; set; }

    /// <summary>
    /// Módulo donde ocurrió la acción
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// Acción realizada
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Resultado de la acción (éxito/error)
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Result { get; set; } = string.Empty;

    /// <summary>
    /// Detalles adicionales de la operación
    /// </summary>
    [StringLength(4000)]
    public string? Detail { get; set; }

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    protected IntranetAuditLog()
    {
    }

    /// <summary>
    /// Factory method para crear un nuevo log de auditoría
    /// </summary>
    public static IntranetAuditLog Create(
        Guid? actorUserId,
        string? actorEmail,
        string module,
        string action,
        string result,
        string? detail)
    {
        return new IntranetAuditLog
        {
            CreatedUtc = DateTime.UtcNow,
            ActorUserId = actorUserId,
            ActorEmail = actorEmail,
            Module = module,
            Action = action,
            Result = result,
            Detail = detail
        };
    }
}
