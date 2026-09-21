namespace TechRiders.Api.Contracts.Responses.Capability;

public sealed class CapabilityRequestResponse
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public required string CapabilityName { get; set; }
    public required string Status { get; set; }
    public required DateTime RequestedAt { get; set; }
    public DateTime? ValidatedAt { get; set; }
    public Guid? ValidatedByUserId { get; set; }
}
