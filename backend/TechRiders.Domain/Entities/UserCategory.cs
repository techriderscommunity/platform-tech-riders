namespace TechRiders.Domain.Entities;

public sealed class UserCategory
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}
