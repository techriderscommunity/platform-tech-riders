using TechRiders.Application.DTOs.Requests.CommunityPartner;
using TechRiders.Application.DTOs.Responses.CommunityPartner;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

public interface ICommunityPartnerApplicationService
{
    Task<CommunityPartnerApplicationResponse> CreateAsync(
        CreateCommunityPartnerApplicationRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>Solicitudes de comunera pendientes de revision por Admin/Staff.</summary>
    Task<List<CommunityPartnerApplication>> GetPendingAsync(CancellationToken cancellationToken = default);

    /// <summary>Aprueba la solicitud: crea la Organization (EntidadColaboradora) y la Community asociada.</summary>
    Task<CommunityPartnerApplication> ApproveAsync(Guid applicationId, Guid validatedByUserId, CancellationToken cancellationToken = default);

    Task<CommunityPartnerApplication> RejectAsync(Guid applicationId, Guid validatedByUserId, CancellationToken cancellationToken = default);
}