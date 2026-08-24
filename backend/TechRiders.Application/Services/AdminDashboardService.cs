using TechRiders.Application.DTOs.Responses.Intranet.AdminDashboard;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

public sealed class AdminDashboardService : IAdminDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIntranetService _intranetService;

    public AdminDashboardService(IUnitOfWork unitOfWork, IIntranetService intranetService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _intranetService = intranetService ?? throw new ArgumentNullException(nameof(intranetService));
    }

    public async Task<AdminDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var allUserCategories = (await _unitOfWork.IntranetUserCategories.GetAllAsync(cancellationToken)).ToArray();
        var activeUserCategories = (await _unitOfWork.IntranetUserCategories.GetActiveAsync(cancellationToken)).ToArray();
        var auditLogs = (await _intranetService.GetAllAuditLogsAsync(cancellationToken))
            .OrderByDescending(item => item.CreatedUtc)
            .Take(5)
            .ToArray();

        var stats = new AdminDashboardStatsResponse
        {
            TotalUsers = allUserCategories.Select(item => item.UserId).Distinct().Count(),
            ActiveUsers = activeUserCategories.Select(item => item.UserId).Distinct().Count(),
            SuperAdmins = allUserCategories
                .Where(item => string.Equals(item.Category, "Admin", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(item.Category, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
                .Select(item => item.UserId)
                .Distinct()
                .Count(),
            Events = await _unitOfWork.Events.CountAsync(cancellationToken: cancellationToken),
            Sessions = await _unitOfWork.Sessions.CountAsync(cancellationToken: cancellationToken),
            Ambassadors = await _unitOfWork.Ambassadors.CountActiveAmbassadorsAsync(cancellationToken),
            JobOffers = await _unitOfWork.Ofertas.CountAsync(cancellationToken: cancellationToken),
            Applications = await _unitOfWork.Candidaturas.CountAsync(cancellationToken: cancellationToken),
        };

        return new AdminDashboardResponse
        {
            Stats = stats,
            RecentActions = auditLogs.Select(item => new AdminDashboardRecentActionResponse
            {
                Action = item.Action,
                Detail = string.IsNullOrWhiteSpace(item.Detail)
                    ? (string.IsNullOrWhiteSpace(item.ActorEmail) ? item.Module : item.ActorEmail)
                    : item.Detail,
                CreatedUtc = item.CreatedUtc,
            }).ToArray(),
            SystemHealth = new AdminDashboardSystemHealthResponse
            {
                Servers = "Online",
                Database = "Healthy",
                Uploads = "No incidents",
                Cpu = "Normal",
            },
        };
    }
}