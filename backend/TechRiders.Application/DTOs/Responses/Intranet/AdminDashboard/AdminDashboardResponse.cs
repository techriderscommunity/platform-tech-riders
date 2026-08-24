namespace TechRiders.Application.DTOs.Responses.Intranet.AdminDashboard;

public sealed class AdminDashboardResponse
{
    public required AdminDashboardStatsResponse Stats { get; init; }

    public required IReadOnlyList<AdminDashboardRecentActionResponse> RecentActions { get; init; }

    public required AdminDashboardSystemHealthResponse SystemHealth { get; init; }
}