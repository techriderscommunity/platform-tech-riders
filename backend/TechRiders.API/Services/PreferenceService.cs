using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Catálogo de taxonomía y preferencias de la persona (Requisitos Arquitectura §5). Una preferencia NUNCA implica consentimiento.</summary>
public static class PreferenceService
{
    public static Task<List<PreferenceDimension>> GetCatalogAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default) =>
        dbContext.Set<PreferenceDimension>()
            .Include(d => d.Values.Where(v => v.IsActive))
            .ToListAsync(cancellationToken);

    public static Task<List<Guid>> GetUserPreferenceValueIdsAsync(TechRidersDbContext dbContext, Guid userId, CancellationToken cancellationToken = default) =>
        dbContext.Set<UserPreference>()
            .Where(p => p.UserId == userId && p.Status == PreferenceStatus.Activa)
            .Select(p => p.DimensionValueId)
            .ToListAsync(cancellationToken);

    /// <summary>Sustituye el conjunto de preferencias activas de la persona por los valores indicados (diff idempotente).</summary>
    public static async Task SetUserPreferencesAsync(TechRidersDbContext dbContext, Guid userId, IReadOnlyCollection<Guid> dimensionValueIds, CancellationToken cancellationToken = default)
    {
        var validValueIds = await dbContext.Set<PreferenceDimensionValue>()
            .Where(v => dimensionValueIds.Contains(v.Id) && v.IsActive)
            .Select(v => v.Id)
            .ToListAsync(cancellationToken);

        var existing = await dbContext.Set<UserPreference>()
            .Where(p => p.UserId == userId)
            .ToListAsync(cancellationToken);

        foreach (var preference in existing.Where(p => p.Status == PreferenceStatus.Activa && !validValueIds.Contains(p.DimensionValueId)))
        {
            preference.Status = PreferenceStatus.Baja;
        }

        var currentActiveValueIds = existing.Where(p => p.Status == PreferenceStatus.Activa).Select(p => p.DimensionValueId).ToHashSet();
        foreach (var valueId in validValueIds.Where(id => !currentActiveValueIds.Contains(id)))
        {
            var reactivated = existing.FirstOrDefault(p => p.DimensionValueId == valueId && p.Status == PreferenceStatus.Baja);
            if (reactivated is not null)
            {
                reactivated.Status = PreferenceStatus.Activa;
                continue;
            }

            await dbContext.Set<UserPreference>().AddAsync(new UserPreference
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DimensionValueId = valueId,
                Status = PreferenceStatus.Activa,
                Origin = "member-self-service",
            }, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
