namespace TechRiders.Domain.Entities;

public sealed class EventType : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<Event> Events { get; set; } = new List<Event>();
}
