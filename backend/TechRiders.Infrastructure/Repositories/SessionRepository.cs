using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

/// <summary>
/// Implementación específica del repositorio de Sesiones
/// Extiende las operaciones básicas con consultas específicas del dominio
/// </summary>
public class SessionRepository : Repository<Session>, ISessionRepository
{
    public SessionRepository(TechRidersDbContext context) : base(context)
    {
    }

    public async Task<Session?> GetSessionWithEventAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Event)
            .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetSessionsByEventIdAsync(
        Guid eventId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.EventId == eventId && s.IsActive)
            .OrderBy(s => s.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetSessionsBySpeakerAsync(
        string speaker, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.IsActive && s.Speaker != null && s.Speaker.Contains(speaker))
            .Include(s => s.Event)
            .OrderBy(s => s.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetSessionsByLevelAsync(
        string level, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.IsActive && s.Level != null && s.Level.Equals(level))
            .Include(s => s.Event)
            .OrderBy(s => s.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetActiveSessionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.IsActive)
            .Include(s => s.Event)
            .OrderBy(s => s.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasTimeConflictAsync(
        Guid eventId, 
        string room, 
        TimeSpan startTime, 
        TimeSpan endTime, 
        Guid? excludeSessionId = null, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(s => 
            s.EventId == eventId &&
            s.IsActive &&
            s.Room != null &&
            s.Room.Equals(room) &&
            ((s.StartTime < endTime && s.EndTime > startTime)));

        if (excludeSessionId.HasValue)
        {
            query = query.Where(s => s.Id != excludeSessionId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
