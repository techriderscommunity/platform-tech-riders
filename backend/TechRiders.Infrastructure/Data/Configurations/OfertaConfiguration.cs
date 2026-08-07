using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities.Empleo;

namespace TechRiders.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad Oferta para Entity Framework Core
/// Define mapeo a tabla SQL, columnas, índices y restricciones
/// </summary>
public sealed class OfertaConfiguration : IEntityTypeConfiguration<Oferta>
{
    public void Configure(EntityTypeBuilder<Oferta> builder)
    {
        builder.ToTable("ofertas", schema: "dbo");
        builder.HasKey(o => o.Id);

        // Primary properties
        builder.Property(o => o.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(o => o.Titulo)
            .HasColumnName("titulo")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(o => o.Empresa)
            .HasColumnName("empresa")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(o => o.Salario)
            .HasColumnName("salario")
            .HasMaxLength(255);

        builder.Property(o => o.Ubicacion)
            .HasColumnName("ubicacion")
            .HasMaxLength(255);

        builder.Property(o => o.Modalidad)
            .HasColumnName("modalidad")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(o => o.Estado)
            .HasColumnName("estado")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(o => o.FechaPublicacion)
            .HasColumnName("fecha_publicacion")
            .IsRequired();

        // Base entity properties
        builder.Property(o => o.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(o => o.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(o => o.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // Concurrency
        builder.Property(o => o.RowVersion)
            .HasColumnName("row_version")
            .IsRowVersion();

        // Indexes
        builder.HasIndex(o => new { o.Estado, o.FechaPublicacion })
            .HasDatabaseName("IX_ofertas_estado_fecha");

        builder.HasIndex(o => o.IsActive)
            .HasDatabaseName("IX_ofertas_is_active");
    }
}
