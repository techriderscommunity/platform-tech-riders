namespace TechRiders.Api.Contracts.Responses.Organizations;

public sealed class OrganizationResponse
{
    public required Guid Id { get; set; }
    public required string OrganizationType { get; set; }
    public required string Name { get; set; }
    public string? TaxId { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public string? Province { get; set; }
    public bool IsActive { get; set; }
}
