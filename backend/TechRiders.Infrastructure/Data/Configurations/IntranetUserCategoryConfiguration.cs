using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities.Intranet;

namespace TechRiders.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad IntranetUserCategory para Entity Framework Core
/// Define mapeo a tabla SQL, columnas, índices y restricciones
/// </summary>
public sealed class IntranetUserCategoryConfiguration : IEntityTypeConfiguration<IntranetUserCategory>
{
    public void Configure(EntityTypeBuilder<IntranetUserCategory> builder)
    {
        builder.ToTable("intranet_user_categories", schema: "dbo");
        builder.HasKey(e => e.Id);

        // Primary properties
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(e => e.Category)
            .HasColumnName("category")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(e => e.Active)
            .HasColumnName("active")
            .HasDefaultValue(true);

        // Base entity properties
        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // Concurrency
        builder.Property(e => e.RowVersion)
            .HasColumnName("row_version")
            .IsRowVersion();

        // Indexes
        builder.HasIndex(e => new { e.UserId, e.Category })
            .IsUnique()
            .HasDatabaseName("IX_intranet_user_categories_user_category_unique");

        builder.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_intranet_user_categories_user_id");

        builder.HasIndex(e => e.Active)
            .HasDatabaseName("IX_intranet_user_categories_active");
    }
}
