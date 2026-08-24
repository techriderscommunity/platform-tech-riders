using TechRiders.Application.DTOs.Responses.Intranet.AdminDashboard;

namespace TechRiders.Application.Interfaces;

public interface IAdminDashboardService
{
    Task<AdminDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken = default);
}