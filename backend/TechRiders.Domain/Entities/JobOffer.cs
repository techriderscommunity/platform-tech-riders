namespace TechRiders.Domain.Entities;

public sealed class JobOffer : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? ContractType { get; set; }
    public string? Url { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public DateTimeOffset? ClosingAt { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
}
