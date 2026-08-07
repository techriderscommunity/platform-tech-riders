using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities.Empleo;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

/// <summary>
/// Implementación de IOfertaRepository
/// Proporciona acceso a datos de Ofertas de Empleo
/// </summary>
public sealed class OfertaRepository : Repository<Oferta>, IOfertaRepository
{
    public OfertaRepository(TechRidersDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets all active job offers
    /// </summary>
    public async Task<IEnumerable<Oferta>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var ofertas = await FindAsync(
            predicate: o => o.IsActive && o.Estado == OfertaEstado.Activa,
            cancellationToken: cancellationToken
        );
        return ofertas.OrderByDescending(o => o.FechaPublicacion);
    }

    /// <summary>
    /// Gets offers by company
    /// </summary>
    public async Task<IEnumerable<Oferta>> GetByEmpresaAsync(string empresa, CancellationToken cancellationToken = default)
    {
        var ofertas = await FindAsync(
            predicate: o => o.IsActive && o.Empresa == empresa,
            cancellationToken: cancellationToken
        );
        return ofertas.OrderByDescending(o => o.FechaPublicacion);
    }

    /// <summary>
    /// Gets offers by work modality
    /// </summary>
    public async Task<IEnumerable<Oferta>> GetByModalidadAsync(Modalidad modalidad, CancellationToken cancellationToken = default)
    {
        var ofertas = await FindAsync(
            predicate: o => o.IsActive && o.Modalidad == modalidad && o.Estado == OfertaEstado.Activa,
            cancellationToken: cancellationToken
        );
        return ofertas.OrderByDescending(o => o.FechaPublicacion);
    }

    /// <summary>
    /// Gets offers within a date range
    /// </summary>
    public async Task<IEnumerable<Oferta>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var ofertas = await FindAsync(
            predicate: o => o.IsActive && o.FechaPublicacion >= startDate && o.FechaPublicacion <= endDate,
            cancellationToken: cancellationToken
        );
        return ofertas.OrderByDescending(o => o.FechaPublicacion);
    }

    /// <summary>
    /// Searches offers by term
    /// </summary>
    public async Task<IEnumerable<Oferta>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var searchQuery = searchTerm.ToLowerInvariant();
        var ofertas = await FindAsync(
            predicate: o => o.IsActive && (
                o.Titulo.ToLower().Contains(searchQuery) ||
                o.Empresa.ToLower().Contains(searchQuery) ||
                o.Ubicacion.ToLower().Contains(searchQuery)
            ),
            cancellationToken: cancellationToken
        );
        return ofertas.OrderByDescending(o => o.FechaPublicacion);
    }
}
