namespace TechRiders.Api.Contracts.Responses.Approvals;

public sealed class ApprovalItemResponse
{
    public required Guid Id { get; set; }
    public required string Type { get; set; }
    public required string Title { get; set; }
    public string? RequestedBy { get; set; }
    public required DateTime RequestedAt { get; set; }
    public required string Module { get; set; }
}
