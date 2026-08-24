namespace TechRiders.Application.DTOs.Responses.Intranet.AdminDashboard;

public sealed class AdminDashboardRecentActionResponse
{
    public string Action { get; init; } = string.Empty;

    public string Detail { get; init; } = string.Empty;

    public DateTime CreatedUtc { get; init; }
}
