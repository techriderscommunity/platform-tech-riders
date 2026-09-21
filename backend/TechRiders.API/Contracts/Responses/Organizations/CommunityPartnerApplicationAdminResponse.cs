namespace TechRiders.Api.Contracts.Responses.Organizations;

public sealed class CommunityPartnerApplicationAdminResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Website { get; set; }
    public required string ContactName { get; set; }
    public required string ContactEmail { get; set; }
    public required string Status { get; set; }
    public required DateTime RequestedAt { get; set; }
}
