using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Pertenencia a la comunidad Tech Riders, independiente del perfil y de las capacidades (Requisitos Arquitectura §3.2).</summary>
public sealed class Membership : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public MembershipStatus Status { get; set; } = MembershipStatus.Pendiente;

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ActivatedAt { get; set; }
    public DateTime? SuspendedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    public string? Origin { get; set; }
    public string? Reason { get; set; }
    public Guid? ValidatedByUserId { get; set; }
}
