namespace TechRiders.Domain.Entities;

public sealed class Community : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Website { get; set; }
    public string? LogoUrl { get; set; }
    public string? LinkedIn { get; set; }
    public string? Instagram { get; set; }

    public Guid? ContactUserId { get; set; }
    public User? ContactUser { get; set; }

    public ICollection<CommunityMember> Members { get; set; } = new List<CommunityMember>();
    public ICollection<CommunityCollaboration> Collaborations { get; set; } = new List<CommunityCollaboration>();
}
