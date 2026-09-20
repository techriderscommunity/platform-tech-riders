using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechRiders.Api.Services;
using TechRiders.Application.DTOs.Responses.Intranet.AdminDashboard;
using TechRiders.Application.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin/dashboard")]
[Produces("application/json")]
public sealed class AdminDashboardController : BaseApiController
{
    private readonly IAdminDashboardService _adminDashboardService;
    private readonly TechRidersDbContext _dbContext;
    private readonly ILogger<AdminDashboardController> _logger;

    public AdminDashboardController(IAdminDashboardService adminDashboardService, TechRidersDbContext dbContext, ILogger<AdminDashboardController> logger)
    {
        _adminDashboardService = adminDashboardService ?? throw new ArgumentNullException(nameof(adminDashboardService));
        _dbContext = dbContext;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdminDashboardResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        try
        {
            var dashboard = await _adminDashboardService.GetDashboardAsync(cancellationToken);
            var now = DateTime.UtcNow;

            var pendingCounts = await ApprovalsAggregationService.GetPendingCountsByTypeAsync(_dbContext, cancellationToken);
            var upcomingEvents = await _dbContext.Set<Domain.Entities.Event>()
                .Where(e => e.IsActive && e.StartDateTime >= now)
                .OrderBy(e => e.StartDateTime)
                .Take(5)
                .Select(e => new AdminDashboardUpcomingItemResponse { Id = e.Id, Title = e.Name, StartDateTime = e.StartDateTime })
                .ToListAsync(cancellationToken);
            var upcomingSessions = await _dbContext.Set<Domain.Entities.Session>()
                .Where(s => s.IsActive && s.StartDateTime >= now)
                .OrderBy(s => s.StartDateTime)
                .Take(5)
                .Select(s => new AdminDashboardUpcomingItemResponse { Id = s.Id, Title = s.Title, StartDateTime = s.StartDateTime })
                .ToListAsync(cancellationToken);

            var extended = new AdminDashboardResponse
            {
                Stats = dashboard.Stats,
                RecentActions = dashboard.RecentActions,
                SystemHealth = dashboard.SystemHealth,
                PendingApprovals = pendingCounts.Select(kv => new AdminDashboardPendingApprovalResponse { Type = kv.Key, Count = kv.Value }).ToList(),
                UpcomingEvents = upcomingEvents,
                UpcomingSessions = upcomingSessions,
            };

            return Ok(extended);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin dashboard");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }
}