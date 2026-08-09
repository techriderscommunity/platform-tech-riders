using TechRiders.Application.DTOs.Responses.Intranet;

namespace TechRiders.Application.Interfaces;

public interface IAdminDashboardService
{
    Task<AdminDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken = default);
}