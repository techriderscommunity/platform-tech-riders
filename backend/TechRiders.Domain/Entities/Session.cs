namespace TechRiders.Domain.Entities;

public sealed class Session : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset? EndDateTime { get; set; }

    // Compatibilidad con contratos legacy de la capa de aplicación
    public TimeSpan StartTime
    {
        get => StartDateTime.TimeOfDay;
        set => StartDateTime = new DateTimeOffset(StartDateTime.Date + value, StartDateTime.Offset);
    }

    public TimeSpan EndTime
    {
        get => EndDateTime?.TimeOfDay ?? StartDateTime.TimeOfDay;
        set => EndDateTime = new DateTimeOffset(StartDateTime.Date + value, StartDateTime.Offset);
    }

    public string? Speaker { get; set; }
    public string? Room { get; set; }
    public string? Level { get; set; }
    public int? MaxCapacity { get; set; }
    public int? StudentCount { get; set; }

    public Guid EventId { get; set; }
    public Event Event { get; set; } = default!;

    public Guid? CenterId { get; set; }
    public Center? Center { get; set; }

    public Guid? StatusId { get; set; }
    public Status? Status { get; set; }

    public ICollection<SessionSpeaker> Speakers { get; set; } = new List<SessionSpeaker>();
    public ICollection<SessionCategory> Categories { get; set; } = new List<SessionCategory>();
    public ICollection<SessionSkill> Skills { get; set; } = new List<SessionSkill>();
    public ICollection<SessionRegistration> Registrations { get; set; } = new List<SessionRegistration>();
}
