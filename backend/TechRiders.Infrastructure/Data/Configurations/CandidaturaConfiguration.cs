using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities.Empleo;

namespace TechRiders.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad Candidatura para Entity Framework Core
/// Define mapeo a tabla SQL, columnas, índices, relaciones y restricciones
/// </summary>
public sealed class CandidaturaConfiguration : IEntityTypeConfiguration<Candidatura>
{
    public void Configure(EntityTypeBuilder<Candidatura> builder)
    {
        builder.ToTable("candidaturas", schema: "dbo");
        builder.HasKey(c => c.Id);

        // Primary properties
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.OfertaId)
            .HasColumnName("oferta_id")
            .IsRequired();

        builder.Property(c => c.JuniorId)
            .HasColumnName("junior_id")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.NombreJunior)
            .HasColumnName("nombre_junior")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.EmailJunior)
            .HasColumnName("email_junior")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Estado)
            .HasColumnName("estado")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(c => c.FechaSolicitud)
            .HasColumnName("fecha_solicitud")
            .IsRequired();

        // Base entity properties
        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // Concurrency
        builder.Property(c => c.RowVersion)
            .HasColumnName("row_version")
            .IsRowVersion();

        // Relationships
        builder.HasOne(c => c.Oferta)
            .WithMany()
            .HasForeignKey(c => c.OfertaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(c => c.OfertaId)
            .HasDatabaseName("IX_candidaturas_oferta_id");

        builder.HasIndex(c => new { c.OfertaId, c.JuniorId })
            .IsUnique()
            .HasDatabaseName("IX_candidaturas_oferta_junior_unique");

        builder.HasIndex(c => c.JuniorId)
            .HasDatabaseName("IX_candidaturas_junior_id");

        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_candidaturas_is_active");
    }
}
