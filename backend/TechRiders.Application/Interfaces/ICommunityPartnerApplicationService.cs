using TechRiders.Application.DTOs.Requests.CommunityPartner;
using TechRiders.Application.DTOs.Responses.CommunityPartner;

namespace TechRiders.Application.Interfaces;

public interface ICommunityPartnerApplicationService
{
    Task<CommunityPartnerApplicationResponse> CreateAsync(
        CreateCommunityPartnerApplicationRequest request,
        CancellationToken cancellationToken = default);
}