using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class intranet_settings
{
    public Guid id { get; set; }

    public string key { get; set; } = null!;

    public string module { get; set; } = null!;

    public string value { get; set; } = null!;

    public string status { get; set; } = null!;

    public DateTime updated_utc { get; set; }

    public string? updated_by { get; set; }

    public byte[] row_version { get; set; } = null!;

    public DateTime created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public bool is_active { get; set; }
}
