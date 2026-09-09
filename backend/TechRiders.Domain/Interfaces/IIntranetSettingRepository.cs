using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

public interface IIntranetSettingRepository : IRepository<IntranetSetting>
{
    Task<IntranetSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);

    Task<IEnumerable<IntranetSetting>> GetByModuleAsync(string module, CancellationToken cancellationToken = default);

    Task<IEnumerable<IntranetSetting>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task<IntranetSetting?> GetByModuleAndKeyAsync(string module, string key, CancellationToken cancellationToken = default);

    Task<bool> KeyExistsAsync(string key, Guid? excludeId = null, CancellationToken cancellationToken = default);
}