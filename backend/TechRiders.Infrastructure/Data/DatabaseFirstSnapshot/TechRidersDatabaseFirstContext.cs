using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot;

public partial class TechRidersDatabaseFirstContext : DbContext
{
    public TechRidersDatabaseFirstContext(DbContextOptions<TechRidersDatabaseFirstContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ambassadors> Ambassadors { get; set; }

    public virtual DbSet<Centers> Centers { get; set; }

    public virtual DbSet<Events> Events { get; set; }

    public virtual DbSet<FPTours> FPTours { get; set; }

    public virtual DbSet<MT_Categories> MT_Categories { get; set; }

    public virtual DbSet<Sessions> Sessions { get; set; }

    public virtual DbSet<candidaturas> candidaturas { get; set; }

    public virtual DbSet<intranet_audit_logs> intranet_audit_logs { get; set; }

    public virtual DbSet<intranet_settings> intranet_settings { get; set; }

    public virtual DbSet<intranet_user_categories> intranet_user_categories { get; set; }

    public virtual DbSet<ofertas> ofertas { get; set; }

    public virtual DbSet<tutoriales> tutoriales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ambassadors>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Ambassadors_CategoryId");

            entity.HasIndex(e => e.Email, "IX_Ambassadors_Email");

            entity.HasIndex(e => e.IsActive, "IX_Ambassadors_IsActive");

            entity.HasIndex(e => e.IsWorking, "IX_Ambassadors_IsWorking");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.About).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Github).HasMaxLength(300);
            entity.Property(e => e.Instagram).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.LinkedIn).HasMaxLength(300);
            entity.Property(e => e.Locality).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Nickname).HasMaxLength(100);
            entity.Property(e => e.OtherCategory).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Skill).HasMaxLength(1000);

            entity.HasOne(d => d.Category).WithMany(p => p.Ambassadors)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Centers>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Centers_Email");

            entity.HasIndex(e => e.IsActive, "IX_Centers_IsActive");

            entity.HasIndex(e => e.Locality, "IX_Centers_Locality");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ContactPerson).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Instagram).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LinkedIn).HasMaxLength(300);
            entity.Property(e => e.Locality).HasMaxLength(200);
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Parking).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Specialty).HasMaxLength(500);
            entity.Property(e => e.Studies).HasMaxLength(1000);
        });

        modelBuilder.Entity<Events>(entity =>
        {
            entity.HasIndex(e => e.IsActive, "IX_Events_IsActive");

            entity.HasIndex(e => e.StartDate, "IX_Events_StartDate");

            entity.HasIndex(e => new { e.StartDate, e.EndDate }, "IX_Events_StartDate_EndDate");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Location).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<FPTours>(entity =>
        {
            entity.HasIndex(e => e.AmbassadorId, "IX_FPTours_AmbassadorId");

            entity.HasIndex(e => e.CenterId, "IX_FPTours_CenterId");

            entity.HasIndex(e => e.HasScheduledDate, "IX_FPTours_HasScheduledDate");

            entity.HasIndex(e => e.IsActive, "IX_FPTours_IsActive");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Ambassador).WithMany(p => p.FPTours)
                .HasForeignKey(d => d.AmbassadorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Center).WithMany(p => p.FPTours)
                .HasForeignKey(d => d.CenterId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MT_Categories>(entity =>
        {
            entity.HasIndex(e => e.Active, "IX_MT_Categories_Active");

            entity.HasIndex(e => e.FatherId, "IX_MT_Categories_FatherId");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Father).WithMany(p => p.InverseFather).HasForeignKey(d => d.FatherId);
        });

        modelBuilder.Entity<Sessions>(entity =>
        {
            entity.HasIndex(e => e.EventId, "IX_Sessions_EventId");

            entity.HasIndex(e => new { e.EventId, e.StartTime }, "IX_Sessions_EventId_StartTime");

            entity.HasIndex(e => e.IsActive, "IX_Sessions_IsActive");

            entity.HasIndex(e => e.Speaker, "IX_Sessions_Speaker");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Level).HasMaxLength(50);
            entity.Property(e => e.Room).HasMaxLength(100);
            entity.Property(e => e.Speaker).HasMaxLength(150);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Event).WithMany(p => p.Sessions).HasForeignKey(d => d.EventId);
        });

        modelBuilder.Entity<candidaturas>(entity =>
        {
            entity.HasIndex(e => e.is_active, "IX_candidaturas_is_active");

            entity.HasIndex(e => e.junior_id, "IX_candidaturas_junior_id");

            entity.HasIndex(e => e.oferta_id, "IX_candidaturas_oferta_id");

            entity.HasIndex(e => new { e.oferta_id, e.junior_id }, "IX_candidaturas_oferta_junior_unique").IsUnique();

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.email_junior).HasMaxLength(255);
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.junior_id).HasMaxLength(255);
            entity.Property(e => e.nombre_junior).HasMaxLength(255);
            entity.Property(e => e.row_version)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.oferta).WithMany(p => p.candidaturas)
                .HasForeignKey(d => d.oferta_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<intranet_audit_logs>(entity =>
        {
            entity.HasIndex(e => e.actor_user_id, "IX_intranet_audit_logs_actor_user_id");

            entity.HasIndex(e => e.created_utc, "IX_intranet_audit_logs_created_utc");

            entity.HasIndex(e => new { e.module, e.action }, "IX_intranet_audit_logs_module_action");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.action).HasMaxLength(255);
            entity.Property(e => e.actor_email).HasMaxLength(255);
            entity.Property(e => e.detail).HasMaxLength(4000);
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.module).HasMaxLength(255);
            entity.Property(e => e.result).HasMaxLength(50);
        });

        modelBuilder.Entity<intranet_settings>(entity =>
        {
            entity.HasIndex(e => e.key, "IX_intranet_settings_key_unique").IsUnique();

            entity.HasIndex(e => e.module, "IX_intranet_settings_module");

            entity.HasIndex(e => e.status, "IX_intranet_settings_status");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.key).HasMaxLength(255);
            entity.Property(e => e.module).HasMaxLength(255);
            entity.Property(e => e.row_version)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.status)
                .HasMaxLength(50)
                .HasDefaultValue("activo");
            entity.Property(e => e.updated_by).HasMaxLength(255);
            entity.Property(e => e.value).HasMaxLength(4000);
        });

        modelBuilder.Entity<intranet_user_categories>(entity =>
        {
            entity.HasIndex(e => e.active, "IX_intranet_user_categories_active");

            entity.HasIndex(e => new { e.user_id, e.category }, "IX_intranet_user_categories_user_category_unique").IsUnique();

            entity.HasIndex(e => e.user_id, "IX_intranet_user_categories_user_id");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.category).HasMaxLength(255);
            entity.Property(e => e.description).HasMaxLength(1000);
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.row_version)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<ofertas>(entity =>
        {
            entity.HasIndex(e => new { e.estado, e.fecha_publicacion }, "IX_ofertas_estado_fecha");

            entity.HasIndex(e => e.is_active, "IX_ofertas_is_active");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.empresa).HasMaxLength(255);
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.row_version)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.salario).HasMaxLength(255);
            entity.Property(e => e.titulo).HasMaxLength(255);
            entity.Property(e => e.ubicacion).HasMaxLength(255);
        });

        modelBuilder.Entity<tutoriales>(entity =>
        {
            entity.HasIndex(e => e.autor, "IX_tutoriales_autor");

            entity.HasIndex(e => e.is_active, "IX_tutoriales_is_active");

            entity.HasIndex(e => e.slug, "IX_tutoriales_slug_unique").IsUnique();

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.autor).HasMaxLength(255);
            entity.Property(e => e.categorias_json)
                .HasMaxLength(2000)
                .HasDefaultValue("[]");
            entity.Property(e => e.extracto).HasMaxLength(1000);
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.row_version)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.slug).HasMaxLength(255);
            entity.Property(e => e.titulo).HasMaxLength(255);
            entity.Property(e => e.url).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
