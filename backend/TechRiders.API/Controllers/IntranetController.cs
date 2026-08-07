using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class IntranetController : ControllerBase
{
    private readonly IIntranetService _intranetService;
    private readonly ILogger<IntranetController> _logger;

    public IntranetController(IIntranetService intranetService, ILogger<IntranetController> logger)
    {
        _intranetService = intranetService ?? throw new ArgumentNullException(nameof(intranetService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("audit-logs")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<IntranetAuditLogResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllAuditLogs(CancellationToken cancellationToken)
    {
        try
        {
            var logs = await _intranetService.GetAllAuditLogsAsync(cancellationToken);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving audit logs");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("audit-logs/filtered")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<IntranetAuditLogResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFilteredAuditLogs(
        [FromQuery] string? module,
        [FromQuery] string? action,
        [FromQuery] Guid? userId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var logs = await _intranetService.GetFilteredAuditLogsAsync(module, action, userId, startDate, endDate, cancellationToken);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving filtered audit logs");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("settings")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<IntranetSettingResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllSettings(CancellationToken cancellationToken)
    {
        try
        {
            var settings = await _intranetService.GetAllSettingsAsync(cancellationToken);
            return Ok(settings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving settings");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("settings/{key}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IntranetSettingResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetSettingByKey(string key, CancellationToken cancellationToken)
    {
        try
        {
            var setting = await _intranetService.GetSettingByKeyAsync(key, cancellationToken);
            if (setting == null)
                return NotFound(new { error = "Setting not found" });

            return Ok(setting);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving setting {Key}", key);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("user-categories/{userId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<IntranetUserCategoryResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserCategories(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var categories = await _intranetService.GetUserCategoriesAsync(userId, cancellationToken);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user categories for {UserId}", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }

    [HttpGet("user-categories/{userId}/has/{category}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UserHasCategory(Guid userId, string category, CancellationToken cancellationToken)
    {
        try
        {
            var hasCategory = await _intranetService.UserHasCategoryAsync(userId, category, cancellationToken);
            return Ok(new { hasCategory });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking user category for {UserId}", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }
}
