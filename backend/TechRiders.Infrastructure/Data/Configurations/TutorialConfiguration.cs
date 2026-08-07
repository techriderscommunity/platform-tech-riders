using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities.Tutoriales;

namespace TechRiders.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad Tutorial para Entity Framework Core
/// Define mapeo a tabla SQL, columnas, índices y restricciones
/// Nota: Categorias se almacena como JSON en la columna categorias_json
/// </summary>
public sealed class TutorialConfiguration : IEntityTypeConfiguration<Tutorial>
{
    public void Configure(EntityTypeBuilder<Tutorial> builder)
    {
        builder.ToTable("tutoriales", schema: "dbo");
        builder.HasKey(t => t.Id);

        // Primary properties
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(t => t.Slug)
            .HasColumnName("slug")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.Titulo)
            .HasColumnName("titulo")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.Extracto)
            .HasColumnName("extracto")
            .HasMaxLength(1000);

        builder.Property(t => t.Autor)
            .HasColumnName("autor")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.FechaPublicacion)
            .HasColumnName("fecha_publicacion")
            .IsRequired();

        builder.Property(t => t.CategoriasJson)
            .HasColumnName("categorias_json")
            .HasMaxLength(2000)
            .HasDefaultValue("[]");

        builder.Property(t => t.Url)
            .HasColumnName("url")
            .HasMaxLength(500);

        // Base entity properties
        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(t => t.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // Concurrency
        builder.Property(t => t.RowVersion)
            .HasColumnName("row_version")
            .IsRowVersion();

        // Indexes
        builder.HasIndex(t => t.Slug)
            .IsUnique()
            .HasDatabaseName("IX_tutoriales_slug_unique");

        builder.HasIndex(t => t.Autor)
            .HasDatabaseName("IX_tutoriales_autor");

        builder.HasIndex(t => t.IsActive)
            .HasDatabaseName("IX_tutoriales_is_active");
    }
}
