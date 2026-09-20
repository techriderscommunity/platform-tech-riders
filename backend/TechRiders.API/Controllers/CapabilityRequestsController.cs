using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Capability;
using TechRiders.Api.Contracts.Responses.Capability;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

/// <summary>Solicitud y aprobación de ascenso de Member a un rol de comunidad (Staff, Community Leader, Ambassador, Center, Community Partner).</summary>
[ApiController]
[Route("api/capability-requests")]
[Produces("application/json")]
[Authorize]
public sealed class CapabilityRequestsController : BaseApiController
{
    private readonly ICapabilityRequestService _capabilityRequestService;
    private readonly ILogger<CapabilityRequestsController> _logger;

    public CapabilityRequestsController(ICapabilityRequestService capabilityRequestService, ILogger<CapabilityRequestsController> logger)
    {
        _capabilityRequestService = capabilityRequestService;
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
            var created = await _capabilityRequestService.RequestAsync(userId.Value, request.CapabilityName, cancellationToken);
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
        var pending = await _capabilityRequestService.GetPendingAsync(cancellationToken);
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
            await _capabilityRequestService.ApproveAsync(id, validatorId.Value, cancellationToken);
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
            await _capabilityRequestService.RejectAsync(id, validatorId.Value, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpPost("{id:guid}/revoke")]
    [Authorize(Policy = "permission:role-requests.approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Revoke(Guid id, CancellationToken cancellationToken)
    {
        var validatorId = GetCurrentUserId();
        if (validatorId is null)
        {
            return Unauthorized();
        }

        try
        {
            await _capabilityRequestService.RevokeAsync(id, validatorId.Value, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpGet("history")]
    [Authorize(Policy = "permission:role-requests.approve")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CapabilityRequestResponse>))]
    public async Task<IActionResult> GetHistory([FromQuery] string? capabilityName, [FromQuery] string? status, CancellationToken cancellationToken)
    {
        Domain.Enums.CapabilityStatus? parsedStatus = null;
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<Domain.Enums.CapabilityStatus>(status, ignoreCase: true, out var parsed))
        {
            parsedStatus = parsed;
        }

        var history = await _capabilityRequestService.GetHistoryAsync(capabilityName, parsedStatus, cancellationToken);
        var response = history.Select(uc => new CapabilityRequestResponse
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

    private Guid? GetCurrentUserId()
    {
        var raw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(raw, out var id) ? id : null;
    }
}
