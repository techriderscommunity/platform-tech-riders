using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

/// <summary>
/// Implementación específica del repositorio de Eventos
/// Extiende las operaciones básicas con consultas específicas del dominio
/// </summary>
public class EventRepository : Repository<Event>, IEventRepository
{
    public EventRepository(TechRidersDbContext context) : base(context)
    {
    }

    public async Task<Event?> GetEventWithSessionsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Sessions.Where(s => s.IsActive))
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetEventsByDateRangeAsync(
        DateTime startDate, 
        DateTime endDate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.IsActive && 
                        e.StartDate <= endDate && 
                        e.EndDate >= startDate)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetActiveEventsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.IsActive)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetUpcomingEventsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(e => e.IsActive && e.StartDate >= now)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> SearchEventsAsync(
        string searchTerm, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.IsActive && 
                        (e.Name.Contains(searchTerm) || 
                         (e.Description != null && e.Description.Contains(searchTerm))))
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }
}
