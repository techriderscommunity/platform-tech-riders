namespace TechRiders.Application.DTOs.Responses.Intranet.AdminDashboard;

public sealed class AdminDashboardSystemHealthResponse
{
    public string Servers { get; init; } = string.Empty;

    public string Database { get; init; } = string.Empty;

    public string Uploads { get; init; } = string.Empty;

    public string Cpu { get; init; } = string.Empty;
}