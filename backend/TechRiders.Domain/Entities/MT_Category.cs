namespace TechRiders.Domain.Entities;

public sealed class MT_Category
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int? FatherId { get; set; }

    public MT_Category? Main { get; set; }

    public ICollection<MT_Category> Secondary { get; set; } = new List<MT_Category>();

    public bool Active { get; set; } = true;
}