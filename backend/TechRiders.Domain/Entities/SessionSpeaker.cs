namespace TechRiders.Domain.Entities;

public sealed class SessionSpeaker
{
    public Guid SessionId { get; set; }
    public Session Session { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public bool IsMainSpeaker { get; set; }
}
