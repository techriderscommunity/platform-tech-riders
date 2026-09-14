using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Visibilidad de campos del perfil (Requisitos Arquitectura §10). Por defecto Private hasta configuración explícita.</summary>
public static class ProfileVisibilityService
{
    public static Task<List<UserFieldVisibility>> GetForUserAsync(TechRidersDbContext dbContext, Guid userId, CancellationToken cancellationToken = default) =>
        dbContext.Set<UserFieldVisibility>().Where(v => v.UserId == userId).ToListAsync(cancellationToken);

    public static async Task<UserFieldVisibility> SetAsync(TechRidersDbContext dbContext, Guid userId, string fieldKey, string visibility, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<FieldVisibility>(visibility, ignoreCase: true, out var parsedVisibility))
        {
            throw new ArgumentException($"Visibilidad '{visibility}' no reconocida.");
        }

        var entry = await dbContext.Set<UserFieldVisibility>()
            .FirstOrDefaultAsync(v => v.UserId == userId && v.FieldKey == fieldKey, cancellationToken);

        if (entry is null)
        {
            entry = new UserFieldVisibility { Id = Guid.NewGuid(), UserId = userId, FieldKey = fieldKey, Visibility = parsedVisibility };
            await dbContext.Set<UserFieldVisibility>().AddAsync(entry, cancellationToken);
        }
        else
        {
            entry.Visibility = parsedVisibility;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return entry;
    }
}
