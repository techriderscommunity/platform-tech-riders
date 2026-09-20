using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

/// <summary>Solicitudes de ejercicio de derechos de privacidad (Requisitos Arquitectura §8.6). Plazos/procedimiento definitivos pendientes del DPO.</summary>
public interface IPrivacyRequestService
{
    Task<PrivacyRequest> CreateAsync(Guid userId, string requestType, string? channel, CancellationToken cancellationToken = default);
    Task<List<PrivacyRequest>> GetMineAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<PrivacyRequest>> ListAsync(CancellationToken cancellationToken = default);
    Task<PrivacyRequest> ResolveAsync(Guid requestId, Guid responsibleUserId, string status, string? resolution, CancellationToken cancellationToken = default);
}
