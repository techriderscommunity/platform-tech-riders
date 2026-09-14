using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Capability;
using TechRiders.Api.Contracts.Responses.Capability;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

/// <summary>Solicitud y aprobación de ascenso de Member a un rol de comunidad (Staff, Community Leader, Ambassador, Center, Community Partner).</summary>
[ApiController]
[Route("api/capability-requests")]
[Produces("application/json")]
[Authorize]
public sealed class CapabilityRequestsController : BaseApiController
{
    private readonly TechRidersDbContext _dbContext;
    private readonly ILogger<CapabilityRequestsController> _logger;

    public CapabilityRequestsController(TechRidersDbContext dbContext, ILogger<CapabilityRequestsController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RequestCapability([FromBody] RequestCapabilityRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        try
        {
            var created = await CapabilityRequestService.RequestAsync(_dbContext, userId.Value, request.CapabilityName, cancellationToken);
            return CreatedAtAction(nameof(GetPending), null, new { created.Id });
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpGet("pending")]
    [Authorize(Policy = "permission:role-requests.approve")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CapabilityRequestResponse>))]
    public async Task<IActionResult> GetPending(CancellationToken cancellationToken)
    {
        var pending = await CapabilityRequestService.GetPendingAsync(_dbContext, cancellationToken);
        var response = pending.Select(uc => new CapabilityRequestResponse
        {
            Id = uc.Id,
            UserId = uc.UserId,
            UserName = uc.User is null ? null : $"{uc.User.Name} {uc.User.LastName}",
            UserEmail = uc.User?.Email,
            CapabilityName = uc.Capability.Name,
            Status = uc.Status.ToString(),
            RequestedAt = uc.RequestedAt,
            ValidatedAt = uc.ValidatedAt,
            ValidatedByUserId = uc.ValidatedByUserId,
        });

        return Ok(response);
    }

    [HttpPost("{id:guid}/approve")]
    [Authorize(Policy = "permission:role-requests.approve")]
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
            await CapabilityRequestService.ApproveAsync(_dbContext, id, validatorId.Value, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpPost("{id:guid}/reject")]
    [Authorize(Policy = "permission:role-requests.approve")]
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
            await CapabilityRequestService.RejectAsync(_dbContext, id, validatorId.Value, cancellationToken);
            return Ok();
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
}
