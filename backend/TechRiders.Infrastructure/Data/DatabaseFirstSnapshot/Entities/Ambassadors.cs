using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class Ambassadors
{
    public Guid Id { get; set; }

    public string? Nickname { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Locality { get; set; }

    public bool IsWorking { get; set; }

    public int? CategoryId { get; set; }

    public string? OtherCategory { get; set; }

    public string? About { get; set; }

    public string? Skill { get; set; }

    public string? LinkedIn { get; set; }

    public string? Instagram { get; set; }

    public string? Github { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual MT_Categories? Category { get; set; }

    public virtual ICollection<FPTours> FPTours { get; set; } = new List<FPTours>();
}
