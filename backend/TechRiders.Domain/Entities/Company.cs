namespace TechRiders.Domain.Entities;

public sealed class Company : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Website { get; set; }
    public string? LinkedIn { get; set; }
    public string? LogoUrl { get; set; }

    public Guid? ContactUserId { get; set; }
    public User? ContactUser { get; set; }

    public ICollection<JobOffer> JobOffers { get; set; } = new List<JobOffer>();
}
