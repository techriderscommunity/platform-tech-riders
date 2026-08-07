using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class ofertas
{
    public Guid id { get; set; }

    public string titulo { get; set; } = null!;

    public string empresa { get; set; } = null!;

    public string salario { get; set; } = null!;

    public string ubicacion { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Requisitos { get; set; } = null!;

    public string modalidad { get; set; } = null!;

    public string estado { get; set; } = null!;

    public DateTime fecha_publicacion { get; set; }

    public byte[] row_version { get; set; } = null!;

    public DateTime created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public bool is_active { get; set; }

    public virtual ICollection<candidaturas> candidaturas { get; set; } = new List<candidaturas>();
}
