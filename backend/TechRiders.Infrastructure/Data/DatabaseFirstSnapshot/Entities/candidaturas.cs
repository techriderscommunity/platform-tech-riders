using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class candidaturas
{
    public Guid id { get; set; }

    public Guid oferta_id { get; set; }

    public string junior_id { get; set; } = null!;

    public string nombre_junior { get; set; } = null!;

    public string email_junior { get; set; } = null!;

    public string estado { get; set; } = null!;

    public DateTime fecha_solicitud { get; set; }

    public byte[] row_version { get; set; } = null!;

    public DateTime created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public bool is_active { get; set; }

    public virtual ofertas oferta { get; set; } = null!;
}
