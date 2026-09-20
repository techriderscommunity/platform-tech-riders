namespace TechRiders.Api.Contracts.Responses.Users;

public sealed class UserActivityResponse
{
    public required int EventsRegistered { get; set; }
    public required int SessionsRegistered { get; set; }
    public required int SpeakerSessions { get; set; }
    public required int FPToursAsAmbassador { get; set; }
    public required IReadOnlyList<UserAuditActionResponse> RecentActions { get; set; }
}

public sealed class UserAuditActionResponse
{
    public required DateTime CreatedUtc { get; set; }
    public required string Module { get; set; }
    public required string Action { get; set; }
    public required string Result { get; set; }
    public string? Detail { get; set; }
}
