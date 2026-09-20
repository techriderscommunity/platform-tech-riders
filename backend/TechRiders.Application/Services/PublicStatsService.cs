using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Enums;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

public sealed class PublicStatsService : IPublicStatsService
{
    private readonly IUnitOfWork _unitOfWork;

    public PublicStatsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<HomeStatsResponse> GetHomeStatsAsync(CancellationToken cancellationToken = default)
    {
        return new HomeStatsResponse
        {
            ActiveAmbassadors = await _unitOfWork.Ambassadors.CountActiveAmbassadorsAsync(cancellationToken),
            ActiveEvents = await _unitOfWork.Events.CountAsync(e => e.IsActive, cancellationToken),
            ActiveSessions = await _unitOfWork.Sessions.CountAsync(s => s.IsActive, cancellationToken),
            ActiveTrainingCenters = await CountActiveTrainingCentersAsync(cancellationToken),
        };
    }

    public async Task<CentersStatsResponse> GetCentersStatsAsync(CancellationToken cancellationToken = default)
    {
        return new CentersStatsResponse
        {
            ActiveTrainingCenters = await CountActiveTrainingCentersAsync(cancellationToken),
        };
    }

    public async Task<WomanTechStatsResponse> GetWomanTechStatsAsync(CancellationToken cancellationToken = default)
    {
        return new WomanTechStatsResponse
        {
            ActiveAmbassadors = await _unitOfWork.Ambassadors.CountActiveAmbassadorsAsync(cancellationToken),
        };
    }

    public async Task<JoinStatsResponse> GetJoinStatsAsync(CancellationToken cancellationToken = default)
    {
        return new JoinStatsResponse
        {
            ActiveAmbassadors = await _unitOfWork.Ambassadors.CountActiveAmbassadorsAsync(cancellationToken),
        };
    }

    public async Task<OrientaTechStatsResponse> GetOrientaTechStatsAsync(CancellationToken cancellationToken = default)
    {
        return new OrientaTechStatsResponse
        {
            ActiveSessions = await _unitOfWork.Sessions.CountAsync(s => s.IsActive, cancellationToken),
        };
    }

    public async Task<AboutStatsResponse> GetAboutStatsAsync(CancellationToken cancellationToken = default)
    {
        return new AboutStatsResponse
        {
            ActiveAmbassadors = await _unitOfWork.Ambassadors.CountActiveAmbassadorsAsync(cancellationToken),
        };
    }

    private Task<int> CountActiveTrainingCentersAsync(CancellationToken cancellationToken)
    {
        return _unitOfWork.Organizations.CountAsync(
            o => o.IsActive && (o.OrganizationType == OrganizationType.CentroFormacion || o.OrganizationType == OrganizationType.CentroEducativo),
            cancellationToken);
    }
}
