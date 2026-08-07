using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class tutoriales
{
    public Guid id { get; set; }

    public string slug { get; set; } = null!;

    public string titulo { get; set; } = null!;

    public string extracto { get; set; } = null!;

    public string autor { get; set; } = null!;

    public DateTime fecha_publicacion { get; set; }

    public string categorias_json { get; set; } = null!;

    public string url { get; set; } = null!;

    public byte[] row_version { get; set; } = null!;

    public DateTime created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public bool is_active { get; set; }
}
