using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Entities.Empleo;
using TechRiders.Domain.Entities.Tutoriales;
using TechRiders.Domain.Entities.Intranet;
using TechRiders.Infrastructure.Data.Configurations;

namespace TechRiders.Infrastructure.Data;

/// <summary>
/// Contexto de base de datos para TechRiders
/// Configurado con DbContext Pooling para mejorar el rendimiento
/// </summary>
public class TechRidersDbContext : DbContext
{
    public TechRidersDbContext(DbContextOptions<TechRidersDbContext> options) 
        : base(options)
    {
    }

    /// <summary>
    /// DbSet de Eventos
    /// </summary>
    public DbSet<Event> Events => Set<Event>();

    /// <summary>
    /// DbSet de Sesiones
    /// </summary>
    public DbSet<Session> Sessions => Set<Session>();

    /// <summary>
    /// DbSet de Ambassadors
    /// </summary>
    public DbSet<Ambassador> Ambassadors => Set<Ambassador>();

    /// <summary>
    /// DbSet de Centros
    /// </summary>
    public DbSet<Center> Centers => Set<Center>();

    /// <summary>
    /// DbSet de Tours FP
    /// </summary>
    public DbSet<FPTour> FPTours => Set<FPTour>();

    /// <summary>
    /// DbSet de Categorías
    /// </summary>
    public DbSet<MT_Category> Categories => Set<MT_Category>();

    /// <summary>
    /// DbSet de Ofertas de Empleo
    /// </summary>
    public DbSet<Oferta> Ofertas => Set<Oferta>();

    /// <summary>
    /// DbSet de Candidaturas
    /// </summary>
    public DbSet<Candidatura> Candidaturas => Set<Candidatura>();

    /// <summary>
    /// DbSet de Tutoriales
    /// </summary>
    public DbSet<Tutorial> Tutoriales => Set<Tutorial>();

    /// <summary>
    /// DbSet de Audit Logs de Intranet
    /// </summary>
    public DbSet<IntranetAuditLog> IntranetAuditLogs => Set<IntranetAuditLog>();

    /// <summary>
    /// DbSet de Configuración de Intranet
    /// </summary>
    public DbSet<IntranetSetting> IntranetSettings => Set<IntranetSetting>();

    /// <summary>
    /// DbSet de Categorías de Usuario de Intranet
    /// </summary>
    public DbSet<IntranetUserCategory> IntranetUserCategories => Set<IntranetUserCategory>();

    /// <summary>
    /// Configuración del modelo usando Fluent API y Entity Type Configurations
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Evento
        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Events");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description )
                .HasMaxLength(2000);

            entity.Property(e => e.StartDate)
                .IsRequired()
                .HasColumnType("datetime2");

            entity.Property(e => e.EndDate)
                .IsRequired()
                .HasColumnType("datetime2");

            entity.Property(e => e.Location)
                .HasMaxLength(300);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime2");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Índices para mejorar el rendimiento
            entity.HasIndex(e => e.StartDate);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => new { e.StartDate, e.EndDate });

            // Relación uno a muchos con Sesiones
            entity.HasMany(e => e.Sessions)
                .WithOne(s => s.Event)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de Sesion
        modelBuilder.Entity<Session>(entity =>
        {
            entity.ToTable("Sessions");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(s => s.Description)
                .HasMaxLength(2000);

            entity.Property(s => s.StartTime)
                .IsRequired()
                .HasColumnType("time");

            entity.Property(s => s.EndTime)
                .IsRequired()
                .HasColumnType("time");

            entity.Property(s => s.Speaker)
                .HasMaxLength(150);

            entity.Property(s => s.Room)
                .HasMaxLength(100);

            entity.Property(s => s.Level)
                .HasMaxLength(50);

            entity.Property(s => s.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(s => s.UpdatedAt)
                .HasColumnType("datetime2");

            entity.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Índices para mejorar el rendimiento
            entity.HasIndex(s => s.EventId);
            entity.HasIndex(s => s.IsActive);
            entity.HasIndex(s => s.Speaker);
            entity.HasIndex(s => new { s.EventId, s.StartTime });
        });

        // Configuración de MT_Category
        modelBuilder.Entity<MT_Category>(entity =>
        {
            entity.ToTable("MT_Categories");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(c => c.Active)
                .IsRequired()
                .HasDefaultValue(true);

            // Relación auto-referencial
            entity.HasOne(c => c.Main)
                .WithMany(c => c.Secondary)
                .HasForeignKey(c => c.FatherId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(c => c.FatherId);
            entity.HasIndex(c => c.Active);

            // Seed data para categorías
            entity.HasData(
                // Categorías principales
                new MT_Category { Id = 1, Name = "Desarrollo y Programación Software", FatherId = null, Active = true },
                new MT_Category { Id = 2, Name = "Sistemas, Redes e Infraestructura", FatherId = null, Active = true },
                new MT_Category { Id = 3, Name = "Datos e Inteligencia Artificial", FatherId = null, Active = true },
                new MT_Category { Id = 4, Name = "Diseño y Gestión Digital", FatherId = null, Active = true },
                new MT_Category { Id = 5, Name = "Habilidades y Orientación Laboral", FatherId = null, Active = true },

                // Subcategorías de Desarrollo y Programación Software
                new MT_Category { Id = 101, Name = "Programación Frontend", FatherId = 1, Active = true },
                new MT_Category { Id = 102, Name = "Programación Backend", FatherId = 1, Active = true },
                new MT_Category { Id = 103, Name = "Desarrollo Móvil", FatherId = 1, Active = true },
                new MT_Category { Id = 104, Name = "Videojuegos y Entornos 3D", FatherId = 1, Active = true },

                // Subcategorías de Sistemas, Redes e Infraestructura
                new MT_Category { Id = 201, Name = "Sistemas Operativos y Redes", FatherId = 2, Active = true },
                new MT_Category { Id = 202, Name = "Cloud Computing", FatherId = 2, Active = true },
                new MT_Category { Id = 203, Name = "Ciberseguridad y Hacking Ético", FatherId = 2, Active = true },
                new MT_Category { Id = 204, Name = "DevOps y Automatización", FatherId = 2, Active = true },

                // Subcategorías de Datos e Inteligencia Artificial
                new MT_Category { Id = 301, Name = "Inteligencia Artificial Aplicada", FatherId = 3, Active = true },
                new MT_Category { Id = 302, Name = "Ciencia de Datos y Big Data", FatherId = 3, Active = true },
                new MT_Category { Id = 303, Name = "Business Intelligence (BI)", FatherId = 3, Active = true },

                // Subcategorías de Diseño y Gestión Digital
                new MT_Category { Id = 401, Name = "Diseño UX/UI y Prototipado", FatherId = 4, Active = true },
                new MT_Category { Id = 402, Name = "Metodologías Ágiles (Agile)", FatherId = 4, Active = true },
                new MT_Category { Id = 403, Name = "Marketing Digital y Growth", FatherId = 4, Active = true },

                // Subcategorías de Habilidades y Orientación Laboral
                new MT_Category { Id = 501, Name = "Orientación Laboral y Marca Personal", FatherId = 5, Active = true },
                new MT_Category { Id = 502, Name = "Habilidades Blandas (Soft Skills)", FatherId = 5, Active = true },
                new MT_Category { Id = 503, Name = "Emprendimiento y Startups", FatherId = 5, Active = true }
            );
        });

        // Configuración de Ambassador
        modelBuilder.Entity<Ambassador>(entity =>
        {
            entity.ToTable("Ambassadors");
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Nickname).HasMaxLength(100);
            entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
            entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Email).IsRequired().HasMaxLength(200);
            entity.Property(a => a.Phone).HasMaxLength(20);
            entity.Property(a => a.Locality).HasMaxLength(200);
            entity.Property(a => a.OtherCategory).HasMaxLength(200);
            entity.Property(a => a.About).HasMaxLength(2000);
            entity.Property(a => a.Skill).HasMaxLength(1000);
            entity.Property(a => a.LinkedIn).HasMaxLength(300);
            entity.Property(a => a.Instagram).HasMaxLength(300);
            entity.Property(a => a.Github).HasMaxLength(300);

            entity.Property(a => a.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(a => a.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasOne(a => a.Category)
                .WithMany(c => c.Ambassadors)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(a => a.Email);
            entity.HasIndex(a => a.CategoryId);
            entity.HasIndex(a => a.IsActive);
            entity.HasIndex(a => a.IsWorking);
        });

        // Configuración de Center
        modelBuilder.Entity<Center>(entity =>
        {
            entity.ToTable("Centers");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.Property(c => c.ContactPerson).HasMaxLength(200);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Phone).HasMaxLength(20);
            entity.Property(c => c.Locality).HasMaxLength(200);
            entity.Property(c => c.Studies).HasMaxLength(1000);
            entity.Property(c => c.Specialty).HasMaxLength(500);
            entity.Property(c => c.Location).HasMaxLength(500);
            entity.Property(c => c.Parking).HasMaxLength(500);
            entity.Property(c => c.LinkedIn).HasMaxLength(300);
            entity.Property(c => c.Instagram).HasMaxLength(300);

            entity.Property(c => c.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasIndex(c => c.Email);
            entity.HasIndex(c => c.Locality);
            entity.HasIndex(c => c.IsActive);
        });

        // Configuración de FPTour
        modelBuilder.Entity<FPTour>(entity =>
        {
            entity.ToTable("FPTours");
            entity.HasKey(t => t.Id);

            entity.Property(t => t.CenterId).IsRequired();
            entity.Property(t => t.AmbassadorId).IsRequired();

            entity.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasOne(t => t.Center)
                .WithMany(c => c.FPTours)
                .HasForeignKey(t => t.CenterId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Ambassador)
                .WithMany(a => a.FPTours)
                .HasForeignKey(t => t.AmbassadorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(t => t.CenterId);
            entity.HasIndex(t => t.AmbassadorId);
            entity.HasIndex(t => t.HasScheduledDate);
            entity.HasIndex(t => t.IsActive);
        });

        // Aplicar configuraciones de entidades migrables usando IEntityTypeConfiguration
        modelBuilder.ApplyConfiguration(new OfertaConfiguration());
        modelBuilder.ApplyConfiguration(new CandidaturaConfiguration());
        modelBuilder.ApplyConfiguration(new TutorialConfiguration());
        modelBuilder.ApplyConfiguration(new IntranetAuditLogConfiguration());
        modelBuilder.ApplyConfiguration(new IntranetSettingConfiguration());
        modelBuilder.ApplyConfiguration(new IntranetUserCategoryConfiguration());

        // Aplicar convenciones globales
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Convención para nombres de tablas en plural (solo si no tiene ToTable explícito)
            var tableAttribute = entityType.FindAnnotation("Relational:TableName");
            if (tableAttribute == null && !entityType.GetTableName()!.EndsWith("s"))
            {
                entityType.SetTableName(entityType.GetTableName() + "s");
            }

            // Convención para columnas datetime2
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    if (property.GetColumnType() == null)
                    {
                        property.SetColumnType("datetime2");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sobrescribe SaveChanges para actualizar automáticamente UpdatedAt
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Actualiza los timestamps de CreatedAt y UpdatedAt automáticamente
    /// </summary>
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
