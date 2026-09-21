using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

/// <summary>Catalogo de taxonomia y preferencias de la persona (Requisitos Arquitectura §5). Una preferencia NUNCA implica consentimiento.</summary>
public interface IPreferenceService
{
    Task<List<PreferenceDimension>> GetCatalogAsync(CancellationToken cancellationToken = default);
    Task<List<Guid>> GetUserPreferenceValueIdsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task SetUserPreferencesAsync(Guid userId, IReadOnlyCollection<Guid> dimensionValueIds, CancellationToken cancellationToken = default);
}
