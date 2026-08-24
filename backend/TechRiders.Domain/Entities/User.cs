namespace TechRiders.Domain.Entities;

public sealed class User : BaseEntity
{
    public required string Nickname { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiresAt { get; set; }
    public string? Phone { get; set; }
    public string? Locality { get; set; }
    public bool IsWorking { get; set; }
    public DateTimeOffset? LastActivityDate { get; set; }
    public string? GPFId { get; set; }
    public string? About { get; set; }
    public string? LinkedIn { get; set; }
    public string? Instagram { get; set; }
    public string? Github { get; set; }

    public Guid? StatusId { get; set; }
    public Status? Status { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserCategory> UserCategories { get; set; } = new List<UserCategory>();
    public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
    public ICollection<SessionSpeaker> SpeakerSessions { get; set; } = new List<SessionSpeaker>();
    public ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
    public ICollection<SessionRegistration> SessionRegistrations { get; set; } = new List<SessionRegistration>();
    public ICollection<FPTour> FPTours { get; set; } = new List<FPTour>();
}
