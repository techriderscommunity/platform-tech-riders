namespace TechRiders.Api.Contracts.Responses.Gpf;

public sealed class GpfPersonLinkResponse
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public required string CodUnico { get; set; }
    public required string Status { get; set; }
    public required DateTime LinkedAt { get; set; }
    public DateTime? LastQueriedAt { get; set; }
    public Guid? ValidatedByUserId { get; set; }
}
