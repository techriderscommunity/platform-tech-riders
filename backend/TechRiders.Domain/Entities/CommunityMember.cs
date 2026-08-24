namespace TechRiders.Domain.Entities;

public sealed class CommunityMember
{
    public Guid CommunityId { get; set; }
    public Community Community { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public string? Role { get; set; }
}
