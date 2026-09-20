using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Responses.Organizations;
using TechRiders.Api.Services;
using TechRiders.Application.DTOs.Requests.CommunityPartner;
using TechRiders.Application.DTOs.Responses.CommunityPartner;
using TechRiders.Application.Interfaces;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/community-partner-applications")]
public sealed class CommunityPartnerApplicationsController : BaseApiController
{
    private readonly ICommunityPartnerApplicationService applicationService;
    private readonly TechRidersDbContext _dbContext;
    private readonly ILogger<CommunityPartnerApplicationsController> logger;

    public CommunityPartnerApplicationsController(
        ICommunityPartnerApplicationService applicationService,
        TechRidersDbContext dbContext,
        ILogger<CommunityPartnerApplicationsController> logger)
    {
        this.applicationService = applicationService;
        _dbContext = dbContext;
        this.logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    [RequestSizeLimit(32_768)]
    [ProducesResponseType(typeof(CommunityPartnerApplicationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommunityPartnerApplicationResponse>> Create(
        [FromBody] CreateCommunityPartnerApplicationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await applicationService.CreateAsync(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (InvalidOperationException exception)
        {
            logger.LogInformation(exception, "Duplicate community partner application rejected");
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Application already exists",
                Detail = exception.Message,
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid application",
                Detail = exception.Message,
            });
        }
    }

    [HttpGet("pending")]
    [Authorize(Policy = "permission:community.manage")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommunityPartnerApplicationAdminResponse>))]
    public async Task<IActionResult> GetPending(CancellationToken cancellationToken)
    {
        var pending = await CommunityPartnerApplicationAdminService.GetPendingAsync(_dbContext, cancellationToken);
        return Ok(pending.Select(ToAdminResponse));
    }

    [HttpPost("{id:guid}/approve")]
    [Authorize(Policy = "permission:community.manage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
    {
        var validatorId = GetCurrentUserId();
        if (validatorId is null)
        {
            return Unauthorized();
        }

        try
        {
            var application = await CommunityPartnerApplicationAdminService.ApproveAsync(_dbContext, id, validatorId.Value, cancellationToken);
            return Ok(ToAdminResponse(application));
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpPost("{id:guid}/reject")]
    [Authorize(Policy = "permission:community.manage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Reject(Guid id, CancellationToken cancellationToken)
    {
        var validatorId = GetCurrentUserId();
        if (validatorId is null)
        {
            return Unauthorized();
        }

        try
        {
            var application = await CommunityPartnerApplicationAdminService.RejectAsync(_dbContext, id, validatorId.Value, cancellationToken);
            return Ok(ToAdminResponse(application));
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    private Guid? GetCurrentUserId()
    {
        var raw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(raw, out var id) ? id : null;
    }

    private static CommunityPartnerApplicationAdminResponse ToAdminResponse(Domain.Entities.CommunityPartnerApplication application) => new()
    {
        Id = application.Id,
        Name = application.Name,
        Website = application.Website,
        ContactName = application.ContactName,
        ContactEmail = application.ContactEmail,
        Status = application.Status,
        RequestedAt = application.CreatedAt,
    };
}