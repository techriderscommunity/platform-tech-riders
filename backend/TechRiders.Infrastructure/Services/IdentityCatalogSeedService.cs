using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechRiders.Domain.Entities;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>
/// Siembra el catalogo fijo de Perfiles y Capacidades (Requisitos Arquitectura §3.3/§3.5).
/// Idempotente: solo crea los que falten por nombre.
/// </summary>
public static class IdentityCatalogSeedService
{
    public const string VisitanteProfile = "Visitante";
    public const string StaffTajamarProfile = "Staff Tajamar";

    private static readonly string[] ProfileNames =
    [
        VisitanteProfile,
        "Estudiante Tech Activo",
        "Profesor Tech",
        "Orientador",
        "Profesional Tech Junior",
        "Profesional Tech Senior",
        StaffTajamarProfile,
    ];

    // Capacidades = solicitud de ascenso desde Member a uno de los roles de comunidad.
    // Member es automatico (sin solicitud); estos 5 requieren aprobacion de Admin/Staff.
    private static readonly string[] CapabilityNames =
    [
        "Staff",
        "Community Leader",
        "Ambassador",
        "Center",
        "Community Partner",
    ];

    public static async Task EnsureDefaultsAsync(TechRidersDbContext dbContext, ILogger logger, CancellationToken cancellationToken = default)
    {
        var existingProfiles = await dbContext.Set<Profile>().Select(p => p.Name).ToListAsync(cancellationToken);
        foreach (var name in ProfileNames.Except(existingProfiles))
        {
            dbContext.Set<Profile>().Add(new Profile { Id = Guid.NewGuid(), Name = name });
        }

        var existingCapabilities = await dbContext.Set<Capability>().Select(c => c.Name).ToListAsync(cancellationToken);
        foreach (var name in CapabilityNames.Except(existingCapabilities))
        {
            dbContext.Set<Capability>().Add(new Capability { Id = Guid.NewGuid(), Name = name });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Catalogo de perfiles y capacidades verificado ({Profiles} perfiles, {Capabilities} capacidades).", ProfileNames.Length, CapabilityNames.Length);
    }

    public static Task<Profile> GetProfileAsync(TechRidersDbContext dbContext, string name, CancellationToken cancellationToken = default) =>
        dbContext.Set<Profile>().FirstAsync(p => p.Name == name, cancellationToken);
}
