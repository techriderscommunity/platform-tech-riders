using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class Events
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Location { get; set; }

    public int? MaxCapacity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Sessions> Sessions { get; set; } = new List<Sessions>();
}
