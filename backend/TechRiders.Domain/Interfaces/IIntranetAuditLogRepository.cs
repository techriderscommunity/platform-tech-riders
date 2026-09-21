using TechRiders.Domain.Entities;

namespace TechRiders.Domain.Interfaces;

public interface IIntranetAuditLogRepository : IRepository<IntranetAuditLog>
{
    Task<IEnumerable<IntranetAuditLog>> GetByModuleAsync(string module, CancellationToken cancellationToken = default);

    Task<IEnumerable<IntranetAuditLog>> GetByActorUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<IntranetAuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    Task<IEnumerable<IntranetAuditLog>> GetByResultAsync(string result, CancellationToken cancellationToken = default);

    Task<IEnumerable<IntranetAuditLog>> GetFilteredAsync(
        string? module = null,
        string? action = null,
        Guid? userId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);
}