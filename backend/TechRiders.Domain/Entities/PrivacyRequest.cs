using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Solicitud de ejercicio de un derecho de privacidad (acceso, supresión, oposición, baja, etc.). Plazos y procedimiento definitivo pendientes del DPO.</summary>
public sealed class PrivacyRequest : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public PrivacyRequestType RequestType { get; set; }
    public string? Channel { get; set; }
    public PrivacyRequestStatus Status { get; set; } = PrivacyRequestStatus.Recibida;
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public Guid? ResponsibleUserId { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? Resolution { get; set; }
    public string? InternalNotes { get; set; }
}
