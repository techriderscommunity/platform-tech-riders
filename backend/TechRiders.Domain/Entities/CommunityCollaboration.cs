namespace TechRiders.Domain.Entities;

public sealed class CommunityCollaboration : BaseEntity
{
    public Guid CommunityId { get; set; }
    public Community Community { get; set; } = default!;

    public Guid? EventId { get; set; }
    public Event? Event { get; set; }

    public string? Description { get; set; }
}
