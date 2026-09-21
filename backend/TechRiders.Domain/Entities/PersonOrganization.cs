using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Relación histórica entre una persona y una organización (cargo, tipo, vigencia).</summary>
public sealed class PersonOrganization : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public string? Position { get; set; }
    public PersonOrganizationRelationType RelationType { get; set; }
    public bool IsPrimaryContact { get; set; }
    public PersonOrganizationStatus Status { get; set; } = PersonOrganizationStatus.Pendiente;

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? ValidatedByUserId { get; set; }
    public DateTime? ValidatedAt { get; set; }
}
