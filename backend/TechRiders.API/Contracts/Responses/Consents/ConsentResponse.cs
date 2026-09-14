namespace TechRiders.Api.Contracts.Responses.Consents;

public sealed class ConsentPurposeResponse
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}

public sealed class ConsentResponse
{
    public required Guid PurposeId { get; set; }
    public required string PurposeCode { get; set; }
    public required string PurposeName { get; set; }
    public required string Status { get; set; }
    public DateTime? GrantedAt { get; set; }
    public DateTime? WithdrawnAt { get; set; }
}
