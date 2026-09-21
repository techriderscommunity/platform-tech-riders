namespace TechRiders.Domain.Entities;

/// <summary>Valor de una dimensión de preferencia. No se elimina físicamente: se marca IsActive = false cuando deja de usarse.</summary>
public sealed class PreferenceDimensionValue : BaseEntity
{
    public Guid DimensionId { get; set; }
    public PreferenceDimension Dimension { get; set; } = default!;

    public required string Code { get; set; }
    public required string Name { get; set; }

    public Guid? ParentValueId { get; set; }
    public PreferenceDimensionValue? ParentValue { get; set; }
    public ICollection<PreferenceDimensionValue> ChildValues { get; set; } = new List<PreferenceDimensionValue>();

    public ICollection<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();
    public ICollection<ContentClassification> ContentClassifications { get; set; } = new List<ContentClassification>();
}
