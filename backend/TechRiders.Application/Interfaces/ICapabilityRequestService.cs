using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Flujo de "ascenso" de Member a un rol de comunidad (Staff, Community Leader, Ambassador, Center,
/// Community Partner) con aprobacion de Admin/Staff. Member es automatico y no pasa por aqui.
/// </summary>
public interface ICapabilityRequestService
{
    Task<UserCapability> RequestAsync(Guid userId, string capabilityName, CancellationToken cancellationToken = default);
    Task<List<UserCapability>> GetPendingAsync(CancellationToken cancellationToken = default);

    /// <summary>Historico completo de solicitudes (todas las capacidades y estados), opcionalmente filtrado.</summary>
    Task<List<UserCapability>> GetHistoryAsync(string? capabilityName, CapabilityStatus? status, CancellationToken cancellationToken = default);

    /// <summary>Revoca una capacidad previamente activa (p.ej. dar de baja a un Ambassador) y retira el rol de sistema asociado.</summary>
    Task<UserCapability> RevokeAsync(Guid requestId, Guid validatedByUserId, CancellationToken cancellationToken = default);

    Task<UserCapability> ApproveAsync(Guid requestId, Guid validatedByUserId, CancellationToken cancellationToken = default);
    Task<UserCapability> RejectAsync(Guid requestId, Guid validatedByUserId, CancellationToken cancellationToken = default);
}
