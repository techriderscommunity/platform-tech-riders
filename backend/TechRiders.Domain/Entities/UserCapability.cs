using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Capacidad concreta concedida (o solicitada) a una persona, con estado, vigencia y validador.</summary>
public sealed class UserCapability : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid CapabilityId { get; set; }
    public Capability Capability { get; set; } = default!;

    public CapabilityStatus Status { get; set; } = CapabilityStatus.Pendiente;

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ValidatedAt { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public Guid? ValidatedByUserId { get; set; }
}
