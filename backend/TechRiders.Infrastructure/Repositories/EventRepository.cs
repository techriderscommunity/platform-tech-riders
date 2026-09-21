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
            .Include(e => e.Sessions
                .OrderByDescending(s => s.IsActive)
                .ThenByDescending(s => s.StartDateTime))
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
        var today = DateTime.UtcNow.Date;
        var events = await _dbSet
            .Where(e => e.IsActive)
            .ToListAsync(cancellationToken);

        return OrderByAgenda(events, today);
    }

    public async Task<IEnumerable<Event>> GetUpcomingEventsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var events = await _dbSet
            .Where(e => e.IsActive)
            .ToListAsync(cancellationToken);

        return OrderByAgenda(events, today);
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

    private static IEnumerable<Event> OrderByAgenda(IEnumerable<Event> events, DateTime today)
    {
        return events
            .OrderBy(e => e.StartDate.Date < today)
            .ThenBy(e => e.StartDate.Date < today ? DateTime.MaxValue.AddTicks(-e.StartDate.Ticks) : e.StartDate);
    }
}
