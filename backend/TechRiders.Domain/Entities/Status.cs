namespace TechRiders.Domain.Entities;

public sealed class Status : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Scope { get; set; }
}
