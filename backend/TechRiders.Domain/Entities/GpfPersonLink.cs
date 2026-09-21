using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>
/// Vínculo manual y opcional de una persona Tech Riders con su persona equivalente en GPF (CodUnico).
/// Sustituye al uso ad-hoc de <see cref="User.GPFId"/>; máximo un vínculo activo por persona y por CodUnico
/// (Especificación de Relaciones GPF §3, regla de integridad).
/// </summary>
public sealed class GpfPersonLink : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public required string CodUnico { get; set; }
    public GpfLinkStatus Status { get; set; } = GpfLinkStatus.Activo;
    public DateTime LinkedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastQueriedAt { get; set; }
    public string? LinkMethod { get; set; }
    public Guid? ValidatedByUserId { get; set; }
}
