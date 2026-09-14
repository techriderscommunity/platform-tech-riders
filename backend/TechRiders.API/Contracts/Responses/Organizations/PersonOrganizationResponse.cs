namespace TechRiders.Api.Contracts.Responses.Organizations;

public sealed class PersonOrganizationResponse
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
    public required Guid OrganizationId { get; set; }
    public required string OrganizationName { get; set; }
    public string? Position { get; set; }
    public required string RelationType { get; set; }
    public required string Status { get; set; }
    public bool IsPrimaryContact { get; set; }
    public required DateTime RequestedAt { get; set; }
    public DateTime? ValidatedAt { get; set; }
}
