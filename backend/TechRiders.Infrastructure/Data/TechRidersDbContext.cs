using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Infrastructure.Persistence.Configurations;

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
    /// DbSet canonico de usuarios.
    /// Los ambassadors son usuarios con rol ambassador, no una entidad separada.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Alias legacy para consultas de ambassadors.
    /// Se mantiene solo para compatibilidad con consultas de rol; la tabla real sigue siendo Users.
    /// </summary>
    public DbSet<User> Ambassadors => Set<User>();

    /// <summary>
    /// DbSet de Tours FP
    /// </summary>
    public DbSet<FPTour> FPTours => Set<FPTour>();

    /// <summary>
    /// DbSet de Categorías
    /// </summary>
    public DbSet<MT_Category> Categories => Set<MT_Category>();

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

    public DbSet<CommunityPartnerApplication> CommunityPartnerApplications => Set<CommunityPartnerApplication>();

    /// <summary>
    /// DbSet de Artículos de conocimiento (tutoriales)
    /// </summary>
    public DbSet<KnowledgeArticle> KnowledgeArticles => Set<KnowledgeArticle>();

    /// <summary>
    /// Categorías (Guid) usadas por KnowledgeArticle/Event/Session/User. Distinta de la
    /// tabla legacy MT_Category (int) expuesta como <see cref="Categories"/>.
    /// </summary>
    public DbSet<Category> ContentCategories => Set<Category>();

    /// <summary>
    /// Skills jerárquicos usados por KnowledgeArticle/Session/User.
    /// </summary>
    public DbSet<Skill> Skills => Set<Skill>();

    /// <summary>
    /// Estados reutilizables (p.ej. Scope = "KnowledgeArticle").
    /// </summary>
    public DbSet<Status> Statuses => Set<Status>();

    /// <summary>
    /// Membresía a la comunidad Tech Riders (1:1 con Usuario, separada de perfil y capacidades).
    /// </summary>
    public DbSet<Membership> Memberships => Set<Membership>();

    /// <summary>
    /// Catálogo de perfiles principales (Visitante, Estudiante Tech Activo, Profesor Tech, Orientador, etc.).
    /// </summary>
    public DbSet<Profile> Profiles => Set<Profile>();

    /// <summary>
    /// Histórico de perfil principal por persona.
    /// </summary>
    public DbSet<UserProfileHistory> UserProfileHistories => Set<UserProfileHistory>();

    /// <summary>
    /// Catálogo de capacidades acumulables (Tech Rider, Mentor, Creador de contenido, etc.).
    /// </summary>
    public DbSet<Capability> Capabilities => Set<Capability>();

    /// <summary>
    /// Capacidades concretas concedidas o solicitadas por persona.
    /// </summary>
    public DbSet<UserCapability> UserCapabilities => Set<UserCapability>();

    /// <summary>
    /// Estudios actuales de la persona (Estudiante Tech Activo).
    /// </summary>
    public DbSet<PersonStudy> PersonStudies => Set<PersonStudy>();

    /// <summary>
    /// Ámbitos de orientación de la persona (perfil Orientador).
    /// </summary>
    public DbSet<UserOrientationScope> UserOrientationScopes => Set<UserOrientationScope>();

    /// <summary>
    /// Organizaciones (centro, empresa, comunidad, etc.). No es una persona ni un rol de usuario.
    /// </summary>
    public DbSet<Organization> Organizations => Set<Organization>();

    /// <summary>
    /// Relación histórica persona-organización (cargo, tipo, vigencia).
    /// </summary>
    public DbSet<PersonOrganization> PersonOrganizations => Set<PersonOrganization>();

    /// <summary>
    /// Vínculo opcional de una organización con GPF.
    /// </summary>
    public DbSet<OrganizationGpfLink> OrganizationGpfLinks => Set<OrganizationGpfLink>();

    /// <summary>
    /// Vínculo manual y opcional de una persona con su equivalente en GPF (CodUnico).
    /// </summary>
    public DbSet<GpfPersonLink> GpfPersonLinks => Set<GpfPersonLink>();

    /// <summary>
    /// Dimensiones de la taxonomía multidimensional de preferencias.
    /// </summary>
    public DbSet<PreferenceDimension> PreferenceDimensions => Set<PreferenceDimension>();

    /// <summary>
    /// Valores de cada dimensión de preferencia.
    /// </summary>
    public DbSet<PreferenceDimensionValue> PreferenceDimensionValues => Set<PreferenceDimensionValue>();

    /// <summary>
    /// Preferencias de una persona sobre valores de dimensión.
    /// </summary>
    public DbSet<UserPreference> UserPreferences => Set<UserPreference>();

    /// <summary>
    /// Clasificación de contenidos/actividades con la misma taxonomía de preferencias.
    /// </summary>
    public DbSet<ContentClassification> ContentClassifications => Set<ContentClassification>();

    /// <summary>
    /// Catálogo de finalidades de tratamiento.
    /// </summary>
    public DbSet<ConsentPurpose> ConsentPurposes => Set<ConsentPurpose>();

    /// <summary>
    /// Catálogo de bases jurídicas.
    /// </summary>
    public DbSet<LegalBasis> LegalBases => Set<LegalBasis>();

    /// <summary>
    /// Versiones de textos legales.
    /// </summary>
    public DbSet<LegalText> LegalTexts => Set<LegalText>();

    /// <summary>
    /// Consentimientos otorgados o retirados por finalidad.
    /// </summary>
    public DbSet<Consent> Consents => Set<Consent>();

    /// <summary>
    /// Solicitudes de ejercicio de derechos de privacidad.
    /// </summary>
    public DbSet<PrivacyRequest> PrivacyRequests => Set<PrivacyRequest>();

    /// <summary>
    /// Visibilidad configurada por campo de perfil.
    /// </summary>
    public DbSet<UserFieldVisibility> UserFieldVisibilities => Set<UserFieldVisibility>();

    /// <summary>
    /// Configuración del modelo usando Fluent API y Entity Type Configurations
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechRidersDbContext).Assembly);

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

            entity.Property(e => e.StartDateTime)
                .IsRequired()
                .HasColumnType("datetimeoffset");

            entity.Property(e => e.EndDateTime)
                .HasColumnType("datetimeoffset");

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
            entity.HasIndex(e => e.StartDateTime);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => new { e.StartDateTime, e.EndDateTime });

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

            entity.Property(s => s.StartDateTime)
                .IsRequired()
                .HasColumnType("datetimeoffset");

            entity.Property(s => s.EndDateTime)
                .HasColumnType("datetimeoffset");

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
            entity.HasIndex(s => new { s.EventId, s.StartDateTime });
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

        // Configuración de Ambassador dentro del modelo actual: los ambassadors
        // se almacenan como usuarios con rol ambassador y no como entidad independiente.
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Nickname).HasMaxLength(100);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Phone).HasMaxLength(20);
            entity.Property(u => u.Locality).HasMaxLength(200);
            entity.Property(u => u.About).HasMaxLength(2000);
            entity.Property(u => u.LinkedIn).HasMaxLength(300);
            entity.Property(u => u.Instagram).HasMaxLength(300);
            entity.Property(u => u.X).HasMaxLength(300);
            entity.Property(u => u.YouTube).HasMaxLength(300);
            entity.Property(u => u.Github).HasMaxLength(300);

            entity.HasIndex(u => u.Email);
            entity.HasIndex(u => u.IsWorking);
        });

        modelBuilder.Entity<CommunityPartnerApplication>(entity =>
        {
            entity.ToTable("CommunityPartnerApplications");
            entity.HasKey(application => application.Id);

            entity.Property(application => application.Name).IsRequired().HasMaxLength(200);
            entity.Property(application => application.Website).IsRequired().HasMaxLength(300);
            entity.Property(application => application.LogoUrl).HasMaxLength(300);
            entity.Property(application => application.ContactEmail).IsRequired().HasMaxLength(200);
            entity.Property(application => application.ContactName).IsRequired().HasMaxLength(200);
            entity.Property(application => application.WhoYouAre).IsRequired().HasMaxLength(2000);
            entity.Property(application => application.WhatYouDo).IsRequired().HasMaxLength(2000);
            entity.Property(application => application.Mission).IsRequired().HasMaxLength(2000);
            entity.Property(application => application.Topics).IsRequired().HasMaxLength(1000);
            entity.Property(application => application.Scope).IsRequired().HasMaxLength(30);
            entity.Property(application => application.LinkedIn).HasMaxLength(300);
            entity.Property(application => application.Instagram).HasMaxLength(300);
            entity.Property(application => application.X).HasMaxLength(300);
            entity.Property(application => application.YouTube).HasMaxLength(300);
            entity.Property(application => application.Github).HasMaxLength(300);
            entity.Property(application => application.Status).IsRequired().HasMaxLength(30).HasDefaultValue("pending");
            entity.Property(application => application.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(application => application.Name);
            entity.HasIndex(application => application.Website);
            entity.HasIndex(application => application.ContactEmail);
            entity.HasIndex(application => application.Status);
        });

        // Configuración de FPTour
        modelBuilder.Entity<FPTour>(entity =>
        {
            entity.ToTable("FPTours");
            entity.HasKey(t => t.Id);

            entity.Property(t => t.OrganizationId).IsRequired();
            entity.Property(t => t.AmbassadorUserId).IsRequired();

            entity.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasOne(t => t.Organization)
                .WithMany(c => c.FPTours)
                .HasForeignKey(t => t.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Ambassador)
                .WithMany(a => a.FPTours)
                .HasForeignKey(t => t.AmbassadorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(t => t.OrganizationId);
            entity.HasIndex(t => t.AmbassadorUserId);
            entity.HasIndex(t => t.HasScheduledDate);
            entity.HasIndex(t => t.IsActive);
        });

        // Configuraciones de entidades migrables gestionadas directamente en este DbContext.
        // No se aplican clases legacy inexistentes en el modelo actual.

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
