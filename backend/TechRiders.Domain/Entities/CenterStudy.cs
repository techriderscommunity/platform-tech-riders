namespace TechRiders.Domain.Entities;

public sealed class CenterStudy : BaseEntity
{
    public Guid CenterId { get; set; }
    public Center Center { get; set; } = default!;

    public required string Name { get; set; }
    public string? Specialty { get; set; }
}
