using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses.Intranet.AdminDashboard;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin/dashboard")]
[Produces("application/json")]
public sealed class AdminDashboardController : BaseApiController
{
    private readonly IAdminDashboardService _adminDashboardService;
    private readonly ILogger<AdminDashboardController> _logger;

    public AdminDashboardController(IAdminDashboardService adminDashboardService, ILogger<AdminDashboardController> logger)
    {
        _adminDashboardService = adminDashboardService ?? throw new ArgumentNullException(nameof(adminDashboardService));
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
            return Ok(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin dashboard");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }
}