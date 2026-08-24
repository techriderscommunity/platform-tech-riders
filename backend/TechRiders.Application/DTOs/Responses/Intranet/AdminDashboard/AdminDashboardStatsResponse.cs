namespace TechRiders.Application.DTOs.Responses.Intranet.AdminDashboard;

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