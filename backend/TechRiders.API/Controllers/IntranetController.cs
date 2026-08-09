using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Intranet;
using TechRiders.Api.Services;
using TechRiders.Application.DTOs.Responses;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class IntranetController : ControllerBase
{
    private const string SessionWorkflowKey = "intranet-session-workflow";

    private readonly IIntranetService _intranetService;
    private readonly ILogger<IntranetController> _logger;
    private readonly IMvpRuntimeStateStore mvpRuntimeStateStore;

    public IntranetController(
        IIntranetService intranetService,
        ILogger<IntranetController> logger,
        IMvpRuntimeStateStore mvpRuntimeStateStore)
    {
        _intranetService = intranetService ?? throw new ArgumentNullException(nameof(intranetService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mvpRuntimeStateStore = mvpRuntimeStateStore ?? throw new ArgumentNullException(nameof(mvpRuntimeStateStore));
    }

    [HttpGet("perfil")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MemberProfileState))]
    public IActionResult GetMemberProfile([FromQuery] string? userKey, [FromQuery] string? email)
    {
        var profile = mvpRuntimeStateStore.GetOrCreateMemberProfile(userKey ?? email ?? string.Empty, email);
        return Ok(profile);
    }

    [HttpPut("perfil")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MemberProfileState))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveMemberProfile([FromBody] SaveMemberProfileRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var profile = new MemberProfileState
        {
            Name = request.Name,
            Email = request.Email,
            Bio = request.Bio,
            Interests = request.Interests,
            Audience = request.Audience,
            CommunityRole = request.CommunityRole,
            Organization = request.Organization ?? string.Empty,
        };

        mvpRuntimeStateStore.UpsertMemberProfile(request.UserKey ?? request.Email, profile);
        return Ok(profile);
    }

    [HttpGet("ambassador-profile")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AmbassadorPortalState))]
    public IActionResult GetAmbassadorPortal([FromQuery] string? userKey, [FromQuery] string? email)
    {
        var profile = mvpRuntimeStateStore.GetOrCreateAmbassadorPortal(userKey ?? email ?? string.Empty, email);
        return Ok(profile);
    }

    [HttpPut("ambassador-profile")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AmbassadorPortalState))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveAmbassadorPortal([FromBody] SaveAmbassadorPortalRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var profile = new AmbassadorPortalState
        {
            Email = request.Email,
            Bio = request.Bio,
            Specialties = request.Specialties,
            Availability = request.Availability,
        };

        mvpRuntimeStateStore.UpsertAmbassadorPortal(request.UserKey ?? request.Email, profile);
        return Ok(profile);
    }

    [HttpGet("mis-categorias")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
    public IActionResult GetMyCategories([FromQuery] string? userKey)
    {
        var categories = mvpRuntimeStateStore.GetUserCategories(userKey ?? string.Empty);
        return Ok(categories);
    }

    [HttpPut("mis-categorias")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveMyCategories([FromBody] SaveCategoriesRequest request)
    {
        if (request.Categories.Count == 0)
        {
            return BadRequest(new { error = "At least one category is required." });
        }

        mvpRuntimeStateStore.UpsertUserCategories(request.UserKey ?? string.Empty, request.Categories);
        return Ok(request.Categories);
    }

    [HttpPost("trazas")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddTrace([FromBody] SaveTraceRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        mvpRuntimeStateStore.AddTrace(new IntranetTraceEntry
        {
            Kind = request.Kind,
            Route = request.Route,
            Detail = request.Detail,
        });

        return Accepted(new { success = true });
    }

    [HttpGet("session-actions")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDictionary<string, SessionActionState>))]
    public IActionResult GetSessionActions([FromQuery] string? userKey)
    {
        var actions = mvpRuntimeStateStore.GetSessionActions(SessionWorkflowKey);
        return Ok(actions);
    }

    [HttpPut("session-actions")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveSessionActions([FromBody] SaveSessionActionsRequest request)
    {
        if (request.Actions.Count == 0)
        {
            return BadRequest(new { error = "At least one session action is required." });
        }

        var mappedActions = request.Actions.ToDictionary(
            item => item.Key,
            item => new SessionActionState
            {
                SessionId = item.Key,
                Status = item.Value.Status,
                AmbassadorAssignedId = item.Value.AmbassadorAssignedId,
                UpdatedAt = DateTimeOffset.UtcNow,
            },
            StringComparer.OrdinalIgnoreCase);

        mvpRuntimeStateStore.UpsertSessionActions(SessionWorkflowKey, mappedActions);
        return Ok(mappedActions);
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
