using TechRiders.Domain.Entities.Intranet;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para operaciones en el repositorio de Configuración de Intranet
/// </summary>
public interface IIntranetSettingRepository : IRepository<IntranetSetting>
{
    /// <summary>
    /// Obtiene un setting por clave
    /// </summary>
    Task<IntranetSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene settings por módulo
    /// </summary>
    Task<IEnumerable<IntranetSetting>> GetByModuleAsync(string module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene settings activos
    /// </summary>
    Task<IEnumerable<IntranetSetting>> GetActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un setting por módulo y clave
    /// </summary>
    Task<IntranetSetting?> GetByModuleAndKeyAsync(string module, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe una clave
    /// </summary>
    Task<bool> KeyExistsAsync(string key, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
