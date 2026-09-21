using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Siembra Comuñeras iniciales en la tabla publica de comunidades.</summary>
public static class CommunitySeedService
{
    private static readonly (string Name, string Description, string Website, string? LinkedIn, string? Instagram, string? X, string? YouTube, string? Github)[] Defaults =
    [
        (
            "GitHub Community Spain",
            "Comunidad para compartir practicas, tooling y cultura de desarrollo colaborativo.",
            "https://example.org/github-community-spain",
            "https://www.linkedin.com",
            null,
            "https://x.com",
            "https://www.youtube.com",
            null
        ),
        (
            "Commit conf",
            "Comunidad y conferencia orientada a ingenieria de software, arquitectura y cultura tecnica.",
            "https://example.org/commit-conf",
            "https://www.linkedin.com",
            null,
            "https://x.com",
            "https://www.youtube.com",
            null
        ),
        (
            "Global Azure Spain",
            "Capitulo local de la jornada global Azure con enfoque en aprendizaje practico y networking.",
            "https://example.org/global-azure-spain",
            "https://www.linkedin.com",
            null,
            "https://x.com",
            null,
            null
        ),
        (
            "SirviendoCodigo",
            "Comunidad enfocada en desarrollo, calidad de codigo y crecimiento profesional.",
            "https://example.org/sirviendo-codigo",
            "https://www.linkedin.com",
            "https://www.instagram.com",
            null,
            null,
            null
        ),
        (
            "women4tt",
            "Comunidad para potenciar liderazgo, visibilidad y carrera tecnologica de mujeres.",
            "https://example.org/women4tt",
            "https://www.linkedin.com",
            "https://www.instagram.com",
            null,
            null,
            null
        ),
        (
            "adopta un jr",
            "Comunidad orientada a acompanar perfiles junior en su entrada al mercado laboral.",
            "https://example.org/adopta-un-jr",
            "https://www.linkedin.com",
            null,
            "https://x.com",
            null,
            null
        ),
        (
            "Guarandinga TECH",
            "Comunidad iberoamericana que conecta desarrollo, cultura tech y proyectos colaborativos.",
            "https://example.org/guarandinga-tech",
            "https://www.linkedin.com",
            null,
            null,
            "https://www.youtube.com",
            null
        ),
        (
            "SQL Server Espanol",
            "Comunidad de base de datos y data platform centrada en SQL Server y ecosistema Microsoft.",
            "https://example.org/sql-server-espanol",
            "https://www.linkedin.com",
            null,
            "https://x.com",
            null,
            null
        ),
    ];

    public static async Task EnsureDefaultsAsync(TechRidersDbContext dbContext, ILogger logger, CancellationToken cancellationToken = default)
    {
        var existingNames = await dbContext.Set<Community>()
            .Select(community => community.Name)
            .ToListAsync(cancellationToken);

        var existing = new HashSet<string>(existingNames, StringComparer.OrdinalIgnoreCase);
        var created = 0;

        foreach (var community in Defaults.Where(community => !existing.Contains(community.Name)))
        {
            dbContext.Set<Community>().Add(new Community
            {
                Id = Guid.NewGuid(),
                Name = community.Name,
                Description = community.Description,
                Website = community.Website,
                LinkedIn = community.LinkedIn,
                Instagram = community.Instagram,
                X = community.X,
                YouTube = community.YouTube,
                Github = community.Github,
                IsActive = true,
            });
            created++;
        }

        if (created > 0)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        logger.LogInformation("Comuñeras publicas verificadas ({CreatedCount} creadas).", created);
    }
}
