namespace TechRiders.Domain.Entities;

public sealed class CenterContact : BaseEntity
{
    public Guid CenterId { get; set; }
    public Center Center { get; set; } = default!;

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Role { get; set; }
}
