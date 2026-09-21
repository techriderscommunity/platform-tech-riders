namespace TechRiders.Api.Contracts.Responses.Events;

public sealed class EventRegistrationResponse
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public required string Status { get; set; }
    public required bool Attended { get; set; }
}
