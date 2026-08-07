using TechRiders.Domain.Entities.Intranet;

namespace TechRiders.Domain.Interfaces;

/// <summary>
/// Interfaz específica para operaciones en el repositorio de Audit Logs de Intranet
/// </summary>
public interface IIntranetAuditLogRepository : IRepository<IntranetAuditLog>
{
    /// <summary>
    /// Obtiene logs por módulo
    /// </summary>
    Task<IEnumerable<IntranetAuditLog>> GetByModuleAsync(string module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene logs por usuario
    /// </summary>
    Task<IEnumerable<IntranetAuditLog>> GetByActorUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene logs de un rango de fechas
    /// </summary>
    Task<IEnumerable<IntranetAuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene logs por resultado (éxito/error)
    /// </summary>
    Task<IEnumerable<IntranetAuditLog>> GetByResultAsync(string result, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene logs filtrados por múltiples criterios
    /// </summary>
    Task<IEnumerable<IntranetAuditLog>> GetFilteredAsync(
        string? module = null,
        string? action = null,
        Guid? userId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);
}
