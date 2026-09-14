using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Versión de un texto legal (política de privacidad, condiciones, consentimientos específicos). Contenido definitivo pendiente de validación del DPO.</summary>
public sealed class LegalText : BaseEntity
{
    public required string TextType { get; set; }
    public Guid? PurposeId { get; set; }
    public ConsentPurpose? Purpose { get; set; }

    public required string Version { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }

    public DateTime? PublishedAt { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public LegalTextStatus Status { get; set; } = LegalTextStatus.Draft;
    public Guid? ValidatedByUserId { get; set; }
    public DateTime? ValidatedAt { get; set; }
}
