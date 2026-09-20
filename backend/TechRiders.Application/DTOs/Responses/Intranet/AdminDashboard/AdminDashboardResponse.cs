namespace TechRiders.Application.DTOs.Responses.Intranet.AdminDashboard;

public sealed class AdminDashboardResponse
{
    public required AdminDashboardStatsResponse Stats { get; init; }

    public required IReadOnlyList<AdminDashboardRecentActionResponse> RecentActions { get; init; }

    public required AdminDashboardSystemHealthResponse SystemHealth { get; init; }

    public IReadOnlyList<AdminDashboardPendingApprovalResponse> PendingApprovals { get; init; } = [];

    public IReadOnlyList<AdminDashboardUpcomingItemResponse> UpcomingEvents { get; init; } = [];

    public IReadOnlyList<AdminDashboardUpcomingItemResponse> UpcomingSessions { get; init; } = [];
}

public sealed class AdminDashboardPendingApprovalResponse
{
    public required string Type { get; init; }
    public required int Count { get; init; }
}

public sealed class AdminDashboardUpcomingItemResponse
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required DateTimeOffset StartDateTime { get; init; }
}