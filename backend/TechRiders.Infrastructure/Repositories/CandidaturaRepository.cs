using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Repositories;

/// <summary>
/// Implementación de ICandidaturaRepository
/// Proporciona acceso a datos de Candidaturas de Empleados
/// </summary>
public sealed class CandidaturaRepository : Repository<Candidatura>, ICandidaturaRepository
{
    public CandidaturaRepository(TechRidersDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene todas las candidaturas para una oferta
    /// </summary>
    public async Task<IEnumerable<Candidatura>> GetByOfertaIdAsync(Guid ofertaId, CancellationToken cancellationToken = default)
    {
        var candidaturas = await FindAsync(
            predicate: c => c.IsActive && c.OfertaId == ofertaId,
            cancellationToken: cancellationToken
        );
        return candidaturas.OrderByDescending(c => c.FechaSolicitud);
    }

    /// <summary>
    /// Obtiene todas las candidaturas de un junior
    /// </summary>
    public async Task<IEnumerable<Candidatura>> GetByJuniorIdAsync(string juniorId, CancellationToken cancellationToken = default)
    {
        var candidaturas = await FindAsync(
            predicate: c => c.IsActive && c.JuniorId == juniorId,
            cancellationToken: cancellationToken
        );
        return candidaturas.OrderByDescending(c => c.FechaSolicitud);
    }

    /// <summary>
    /// Obtiene candidaturas por estado
    /// </summary>
    public async Task<IEnumerable<Candidatura>> GetByEstadoAsync(CandidaturaEstado estado, CancellationToken cancellationToken = default)
    {
        var candidaturas = await FindAsync(
            predicate: c => c.IsActive && c.Estado == estado,
            cancellationToken: cancellationToken
        );
        return candidaturas.OrderByDescending(c => c.FechaSolicitud);
    }

    /// <summary>
    /// Obtiene todas las candidaturas contratadas para una oferta
    /// </summary>
    public async Task<IEnumerable<Candidatura>> GetContratadasAsync(Guid ofertaId, CancellationToken cancellationToken = default)
    {
        var candidaturas = await FindAsync(
            predicate: c => c.IsActive && c.OfertaId == ofertaId && c.Estado == CandidaturaEstado.Contratada,
            cancellationToken: cancellationToken
        );
        return candidaturas.OrderByDescending(c => c.FechaSolicitud);
    }

    /// <summary>
    /// Verifica si existe una candidatura para una oferta y junior específicos
    /// </summary>
    public async Task<bool> ExisteCandidaturaAsync(Guid ofertaId, string juniorId, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(
            predicate: c => c.IsActive && c.OfertaId == ofertaId && c.JuniorId == juniorId,
            cancellationToken: cancellationToken
        );
    }
}
