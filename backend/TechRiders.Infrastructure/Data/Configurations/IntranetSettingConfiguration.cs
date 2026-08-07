using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities.Intranet;

namespace TechRiders.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad IntranetSetting para Entity Framework Core
/// Define mapeo a tabla SQL, columnas, índices y restricciones
/// </summary>
public sealed class IntranetSettingConfiguration : IEntityTypeConfiguration<IntranetSetting>
{
    public void Configure(EntityTypeBuilder<IntranetSetting> builder)
    {
        builder.ToTable("intranet_settings", schema: "dbo");
        builder.HasKey(e => e.Id);

        // Primary properties
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(e => e.Key)
            .HasColumnName("key")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Module)
            .HasColumnName("module")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnName("value")
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .HasDefaultValue("activo");

        builder.Property(e => e.UpdatedUtc)
            .HasColumnName("updated_utc")
            .IsRequired();

        builder.Property(e => e.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(255);

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
        builder.HasIndex(e => e.Key)
            .IsUnique()
            .HasDatabaseName("IX_intranet_settings_key_unique");

        builder.HasIndex(e => e.Module)
            .HasDatabaseName("IX_intranet_settings_module");

        builder.HasIndex(e => e.Status)
            .HasDatabaseName("IX_intranet_settings_status");
    }
}
