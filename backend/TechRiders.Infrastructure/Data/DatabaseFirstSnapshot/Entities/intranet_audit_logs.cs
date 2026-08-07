using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class intranet_audit_logs
{
    public Guid id { get; set; }

    public DateTime created_utc { get; set; }

    public Guid? actor_user_id { get; set; }

    public string? actor_email { get; set; }

    public string module { get; set; } = null!;

    public string action { get; set; } = null!;

    public string result { get; set; } = null!;

    public string? detail { get; set; }

    public DateTime created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public bool is_active { get; set; }
}
