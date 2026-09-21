namespace TechRiders.Domain.Entities;

public sealed class IntranetAuditLog : BaseEntity
{
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public Guid? ActorUserId { get; set; }

    public required string ActorEmail { get; set; }

    public required string Module { get; set; }

    public required string Action { get; set; }

    public required string Result { get; set; }

    public string? Detail { get; set; }
}