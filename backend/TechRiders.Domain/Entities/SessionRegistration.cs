using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

public sealed class SessionRegistration : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid SessionId { get; set; }
    public Session Session { get; set; } = default!;

    public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.Registered;
    public bool Attended { get; set; }
    public string? Feedback { get; set; }
}
