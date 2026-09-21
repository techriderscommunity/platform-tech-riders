using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Privacy;
using TechRiders.Api.Contracts.Responses.Privacy;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

/// <summary>Solicitudes de ejercicio de derechos de privacidad (Requisitos Arquitectura §8.6). Plazos/procedimiento definitivos pendientes del DPO.</summary>
[ApiController]
[Route("api/privacy-requests")]
[Produces("application/json")]
[Authorize]
public sealed class PrivacyRequestsController : BaseApiController
{
    private readonly IPrivacyRequestService _privacyRequestService;

    public PrivacyRequestsController(IPrivacyRequestService privacyRequestService)
    {
        _privacyRequestService = privacyRequestService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePrivacyRequestRequest request, CancellationToken cancellationToken)
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
            var created = await _privacyRequestService.CreateAsync(userId.Value, request.RequestType, request.Channel, cancellationToken);
            return CreatedAtAction(nameof(GetMine), null, new { created.Id });
        }
        catch (ArgumentException ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PrivacyRequestResponse>))]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var requests = await _privacyRequestService.GetMineAsync(userId.Value, cancellationToken);
        return Ok(requests.Select(ToResponse));
    }

    [HttpGet]
    [Authorize(Policy = "permission:privacy.manage")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PrivacyRequestResponse>))]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var requests = await _privacyRequestService.ListAsync(cancellationToken);
        return Ok(requests.Select(ToResponse));
    }

    [HttpPost("{id:guid}/resolve")]
    [Authorize(Policy = "permission:privacy.manage")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrivacyRequestResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Resolve(Guid id, [FromBody] ResolvePrivacyRequestRequest request, CancellationToken cancellationToken)
    {
        var responsibleId = GetCurrentUserId();
        if (responsibleId is null)
        {
            return Unauthorized();
        }

        try
        {
            var resolved = await _privacyRequestService.ResolveAsync(id, responsibleId.Value, request.Status, request.Resolution, cancellationToken);
            return Ok(ToResponse(resolved));
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    private Guid? GetCurrentUserId()
    {
        var raw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(raw, out var id) ? id : null;
    }

    private static PrivacyRequestResponse ToResponse(Domain.Entities.PrivacyRequest request) => new()
    {
        Id = request.Id,
        UserId = request.UserId,
        UserName = request.User is null ? null : $"{request.User.Name} {request.User.LastName}",
        RequestType = request.RequestType.ToString(),
        Channel = request.Channel,
        Status = request.Status.ToString(),
        RequestedAt = request.RequestedAt,
        ResolvedAt = request.ResolvedAt,
        Resolution = request.Resolution,
    };
}
