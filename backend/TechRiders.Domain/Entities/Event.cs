namespace TechRiders.Domain.Entities;

public sealed class Event : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset? EndDateTime { get; set; }

    // Compatibilidad con contratos legacy de la capa de aplicación
    public DateTime StartDate
    {
        get => StartDateTime.UtcDateTime;
        set => StartDateTime = new DateTimeOffset(value, TimeSpan.Zero);
    }

    public DateTime? EndDate
    {
        get => EndDateTime?.UtcDateTime;
        set => EndDateTime = value is null ? null : new DateTimeOffset(value.Value, TimeSpan.Zero);
    }

    public string? Url { get; set; }
    public string? Location { get; set; }
    public int? MaxCapacity { get; set; }

    public Guid EventTypeId { get; set; }
    public EventType EventType { get; set; } = default!;

    public Guid? StatusId { get; set; }
    public Status? Status { get; set; }

    public Guid? CenterId { get; set; }
    public Center? Center { get; set; }

    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<EventCategory> Categories { get; set; } = new List<EventCategory>();
    public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
    public ICollection<CommunityCollaboration> CommunityCollaborations { get; set; } = new List<CommunityCollaboration>();
}
