using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class MT_Categories
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? FatherId { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Ambassadors> Ambassadors { get; set; } = new List<Ambassadors>();

    public virtual MT_Categories? Father { get; set; }

    public virtual ICollection<MT_Categories> InverseFather { get; set; } = new List<MT_Categories>();
}
