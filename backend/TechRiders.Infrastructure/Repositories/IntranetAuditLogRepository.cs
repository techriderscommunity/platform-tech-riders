using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

/// <summary>
/// Implementación de IIntranetAuditLogRepository
/// Proporciona acceso a datos de Audit Logs de Intranet
/// </summary>
public sealed class IntranetAuditLogRepository : Repository<IntranetAuditLog>, IIntranetAuditLogRepository
{
    public IntranetAuditLogRepository(TechRidersDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets audit logs by module
    /// </summary>
    public async Task<IEnumerable<IntranetAuditLog>> GetByModuleAsync(string module, CancellationToken cancellationToken = default)
    {
        var logs = await FindAsync(
            predicate: a => a.Module == module,
            cancellationToken: cancellationToken
        );
        return logs.OrderByDescending(a => a.CreatedUtc);
    }

    /// <summary>
    /// Gets audit logs by actor user id
    /// </summary>
    public async Task<IEnumerable<IntranetAuditLog>> GetByActorUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var logs = await FindAsync(
            predicate: a => a.ActorUserId == userId,
            cancellationToken: cancellationToken
        );
        return logs.OrderByDescending(a => a.CreatedUtc);
    }

    /// <summary>
    /// Gets audit logs within a date range
    /// </summary>
    public async Task<IEnumerable<IntranetAuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var logs = await FindAsync(
            predicate: a => a.CreatedUtc >= startDate && a.CreatedUtc <= endDate,
            cancellationToken: cancellationToken
        );
        return logs.OrderByDescending(a => a.CreatedUtc);
    }

    /// <summary>
    /// Gets audit logs by result
    /// </summary>
    public async Task<IEnumerable<IntranetAuditLog>> GetByResultAsync(string result, CancellationToken cancellationToken = default)
    {
        var logs = await FindAsync(
            predicate: a => a.Result == result,
            cancellationToken: cancellationToken
        );
        return logs.OrderByDescending(a => a.CreatedUtc);
    }

    /// <summary>
    /// Gets audit logs with multiple filters
    /// </summary>
    public async Task<IEnumerable<IntranetAuditLog>> GetFilteredAsync(
        string? module = null,
        string? action = null,
        Guid? userId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var logs = await FindAsync(
            predicate: log =>
                (string.IsNullOrEmpty(module) || log.Module == module) &&
                (string.IsNullOrEmpty(action) || log.Action == action) &&
                (!userId.HasValue || log.ActorUserId == userId) &&
                (!startDate.HasValue || log.CreatedUtc >= startDate.Value) &&
                (!endDate.HasValue || log.CreatedUtc <= endDate.Value),
            cancellationToken: cancellationToken
        );
        return logs.OrderByDescending(a => a.CreatedUtc);
    }
}
