namespace TechRiders.Domain.Entities;

public sealed class IntranetUserCategory : BaseEntity
{
    public Guid UserId { get; set; }

    public int CategoryId { get; set; }

    public required string Category { get; set; }

    public string? Description { get; set; }

    public bool Active { get; set; } = true;
}