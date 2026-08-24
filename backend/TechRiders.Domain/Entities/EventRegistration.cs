using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

public sealed class EventRegistration : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid EventId { get; set; }
    public Event Event { get; set; } = default!;

    public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.Registered;
    public bool Attended { get; set; }
    public string? Feedback { get; set; }
}
