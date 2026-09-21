using TechRiders.Application.DTOs.Responses.Intranet.AdminDashboard;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

public sealed class AdminDashboardService : IAdminDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIntranetService _intranetService;
    private readonly IApprovalsService _approvalsService;

    public AdminDashboardService(IUnitOfWork unitOfWork, IIntranetService intranetService, IApprovalsService approvalsService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _intranetService = intranetService ?? throw new ArgumentNullException(nameof(intranetService));
        _approvalsService = approvalsService ?? throw new ArgumentNullException(nameof(approvalsService));
    }

    public async Task<AdminDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
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
            Admins = allUserCategories
                .Where(item => string.Equals(item.Category, "Admin", StringComparison.OrdinalIgnoreCase))
                .Select(item => item.UserId)
                .Distinct()
                .Count(),
            Events = await _unitOfWork.Events.CountAsync(cancellationToken: cancellationToken),
            Sessions = await _unitOfWork.Sessions.CountAsync(cancellationToken: cancellationToken),
            Ambassadors = await _unitOfWork.Ambassadors.CountActiveAmbassadorsAsync(cancellationToken),
            JobOffers = 0,
            Applications = 0,
        };

        var pendingCounts = await _approvalsService.GetPendingCountsByTypeAsync(cancellationToken);

        var upcomingEvents = (await _unitOfWork.Events.FindAsync(e => e.IsActive && e.StartDateTime >= now, cancellationToken))
            .OrderBy(e => e.StartDateTime)
            .Take(5)
            .Select(e => new AdminDashboardUpcomingItemResponse { Id = e.Id, Title = e.Name, StartDateTime = e.StartDateTime })
            .ToArray();

        var upcomingSessions = (await _unitOfWork.Sessions.FindAsync(s => s.IsActive && s.StartDateTime >= now, cancellationToken))
            .OrderBy(s => s.StartDateTime)
            .Take(5)
            .Select(s => new AdminDashboardUpcomingItemResponse { Id = s.Id, Title = s.Title, StartDateTime = s.StartDateTime })
            .ToArray();

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
            PendingApprovals = pendingCounts.Select(kv => new AdminDashboardPendingApprovalResponse { Type = kv.Key, Count = kv.Value }).ToArray(),
            UpcomingEvents = upcomingEvents,
            UpcomingSessions = upcomingSessions,
        };
    }
}