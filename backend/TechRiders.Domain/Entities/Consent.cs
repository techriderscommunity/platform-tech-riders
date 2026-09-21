using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Consentimiento de una persona para una finalidad concreta, vinculado a una versión de texto. Un interés/preferencia NUNCA es prueba de consentimiento.</summary>
public sealed class Consent : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid PurposeId { get; set; }
    public ConsentPurpose Purpose { get; set; } = default!;

    public Guid? LegalBasisId { get; set; }
    public LegalBasis? LegalBasisEntity { get; set; }

    public ConsentStatus Status { get; set; } = ConsentStatus.Pendiente;
    public DateTime? GrantedAt { get; set; }
    public DateTime? WithdrawnAt { get; set; }
    public string? TextVersion { get; set; }
    public string? Origin { get; set; }
    public string? Evidence { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public Guid? RegisteredByUserId { get; set; }
}
