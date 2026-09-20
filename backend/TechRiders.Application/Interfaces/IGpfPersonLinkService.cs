using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

/// <summary>Vinculacion manual y opcional de una persona Tech Riders con GPF (CodUnico). Sin matching automatico (Relaciones GPF §4.4).</summary>
public interface IGpfPersonLinkService
{
    Task<GpfPersonLink> LinkAsync(Guid userId, string codUnico, Guid validatedByUserId, CancellationToken cancellationToken = default);
    Task UnlinkAsync(Guid linkId, Guid validatedByUserId, CancellationToken cancellationToken = default);
    Task<List<GpfPersonLink>> ListAsync(CancellationToken cancellationToken = default);
    Task<GpfPersonLink?> GetActiveForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
