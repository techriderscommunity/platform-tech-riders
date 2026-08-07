using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class intranet_user_categories
{
    public Guid id { get; set; }

    public Guid user_id { get; set; }

    public string category { get; set; } = null!;

    public string? description { get; set; }

    public bool active { get; set; }

    public byte[] row_version { get; set; } = null!;

    public DateTime created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public bool is_active { get; set; }
}
