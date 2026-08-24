namespace TechRiders.Domain.Entities;

public sealed class EventCategory
{
    public Guid EventId { get; set; }
    public Event Event { get; set; } = default!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}
