using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class Centers
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ContactPerson { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Locality { get; set; }

    public string? Studies { get; set; }

    public string? Specialty { get; set; }

    public int? NumberStudents { get; set; }

    public string? Location { get; set; }

    public string? Parking { get; set; }

    public string? LinkedIn { get; set; }

    public string? Instagram { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<FPTours> FPTours { get; set; } = new List<FPTours>();
}
