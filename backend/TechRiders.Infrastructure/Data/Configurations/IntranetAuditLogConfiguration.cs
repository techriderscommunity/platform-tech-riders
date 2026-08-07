using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechRiders.Domain.Entities.Intranet;

namespace TechRiders.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de la entidad IntranetAuditLog para Entity Framework Core
/// Define mapeo a tabla SQL, columnas, índices y restricciones
/// </summary>
public sealed class IntranetAuditLogConfiguration : IEntityTypeConfiguration<IntranetAuditLog>
{
    public void Configure(EntityTypeBuilder<IntranetAuditLog> builder)
    {
        builder.ToTable("intranet_audit_logs", schema: "dbo");
        builder.HasKey(e => e.Id);

        // Primary properties
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(e => e.CreatedUtc)
            .HasColumnName("created_utc")
            .IsRequired();

        builder.Property(e => e.ActorUserId)
            .HasColumnName("actor_user_id");

        builder.Property(e => e.ActorEmail)
            .HasColumnName("actor_email")
            .HasMaxLength(255);

        builder.Property(e => e.Module)
            .HasColumnName("module")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Action)
            .HasColumnName("action")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Result)
            .HasColumnName("result")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Detail)
            .HasColumnName("detail")
            .HasMaxLength(4000);

        // Base entity properties (not typically used for audit logs but keeping for consistency)
        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(e => e.CreatedUtc)
            .HasDatabaseName("IX_intranet_audit_logs_created_utc");

        builder.HasIndex(e => new { e.Module, e.Action })
            .HasDatabaseName("IX_intranet_audit_logs_module_action");

        builder.HasIndex(e => e.ActorUserId)
            .HasDatabaseName("IX_intranet_audit_logs_actor_user_id");
    }
}
