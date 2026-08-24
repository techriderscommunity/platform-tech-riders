using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

/// <summary>
/// Implementación de IIntranetSettingRepository
/// Proporciona acceso a datos de Configuración de Intranet
/// </summary>
public sealed class IntranetSettingRepository : Repository<IntranetSetting>, IIntranetSettingRepository
{
    public IntranetSettingRepository(TechRidersDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets a setting by key
    /// </summary>
    public async Task<IntranetSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var settings = await FindAsync(
            predicate: s => s.IsActive && s.Key == key,
            cancellationToken: cancellationToken
        );
        return settings.FirstOrDefault();
    }

    /// <summary>
    /// Gets all settings by module
    /// </summary>
    public async Task<IEnumerable<IntranetSetting>> GetByModuleAsync(string module, CancellationToken cancellationToken = default)
    {
        var settings = await FindAsync(
            predicate: s => s.IsActive && s.Module == module,
            cancellationToken: cancellationToken
        );
        return settings.OrderBy(s => s.Key);
    }

    /// <summary>
    /// Gets all active settings
    /// </summary>
    public async Task<IEnumerable<IntranetSetting>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var settings = await FindAsync(
            predicate: s => s.IsActive,
            cancellationToken: cancellationToken
        );
        return settings.OrderBy(s => s.Module).ThenBy(s => s.Key);
    }

    /// <summary>
    /// Gets a setting by module and key
    /// </summary>
    public async Task<IntranetSetting?> GetByModuleAndKeyAsync(string module, string key, CancellationToken cancellationToken = default)
    {
        var settings = await FindAsync(
            predicate: s => s.IsActive && s.Module == module && s.Key == key,
            cancellationToken: cancellationToken
        );
        return settings.FirstOrDefault();
    }

    /// <summary>
    /// Checks if a key already exists
    /// </summary>
    public async Task<bool> KeyExistsAsync(string key, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(
            predicate: s => s.IsActive && s.Key == key && (!excludeId.HasValue || s.Id != excludeId.Value),
            cancellationToken: cancellationToken
        );
    }
}
