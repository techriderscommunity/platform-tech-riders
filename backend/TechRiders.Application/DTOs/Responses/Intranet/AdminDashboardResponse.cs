namespace TechRiders.Application.DTOs.Responses.Intranet;

public sealed class AdminDashboardResponse
{
    public required AdminDashboardStatsResponse Stats { get; init; }

    public required IReadOnlyList<AdminDashboardRecentActionResponse> RecentActions { get; init; }

    public required AdminDashboardSystemHealthResponse SystemHealth { get; init; }
}

public sealed class AdminDashboardStatsResponse
{
    public int TotalUsers { get; init; }

    public int ActiveUsers { get; init; }

    public int SuperAdmins { get; init; }

    public int Events { get; init; }

    public int Sessions { get; init; }

    public int Ambassadors { get; init; }

    public int JobOffers { get; init; }

    public int Applications { get; init; }
}

public sealed class AdminDashboardRecentActionResponse
{
    public string Action { get; init; } = string.Empty;

    public string Detail { get; init; } = string.Empty;

    public DateTime CreatedUtc { get; init; }
}

public sealed class AdminDashboardSystemHealthResponse
{
    public string Servers { get; init; } = string.Empty;

    public string Database { get; init; } = string.Empty;

    public string Uploads { get; init; } = string.Empty;

    public string Cpu { get; init; } = string.Empty;
}