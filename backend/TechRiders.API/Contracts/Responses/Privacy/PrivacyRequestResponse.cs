namespace TechRiders.Api.Contracts.Responses.Privacy;

public sealed class PrivacyRequestResponse
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
    public required string RequestType { get; set; }
    public string? Channel { get; set; }
    public required string Status { get; set; }
    public required DateTime RequestedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? Resolution { get; set; }
}
