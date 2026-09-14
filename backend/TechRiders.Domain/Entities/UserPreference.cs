using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Preferencia de una persona sobre un valor de dimensión. No implica consentimiento ni autorización de comunicación.</summary>
public sealed class UserPreference : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid DimensionValueId { get; set; }
    public PreferenceDimensionValue DimensionValue { get; set; } = default!;

    public int? Priority { get; set; }
    public PreferenceStatus Status { get; set; } = PreferenceStatus.Activa;
    public string? Origin { get; set; }
}
