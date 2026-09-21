using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Gpf;
using TechRiders.Api.Contracts.Responses.Gpf;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

/// <summary>Vinculación manual y opcional de personas Tech Riders con GPF (CodUnico). Sin matching automático (Relaciones GPF §4.4).</summary>
[ApiController]
[Route("api/gpf-person-links")]
[Produces("application/json")]
[Authorize]
public sealed class GpfPersonLinksController : BaseApiController
{
    private readonly IGpfPersonLinkService _gpfPersonLinkService;

    public GpfPersonLinksController(IGpfPersonLinkService gpfPersonLinkService)
    {
        _gpfPersonLinkService = gpfPersonLinkService;
    }

    [HttpGet]
    [Authorize(Policy = "permission:validations.manage")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GpfPersonLinkResponse>))]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var links = await _gpfPersonLinkService.ListAsync(cancellationToken);
        return Ok(links.Select(ToResponse));
    }

    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GpfPersonLinkResponse))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var link = await _gpfPersonLinkService.GetActiveForUserAsync(userId.Value, cancellationToken);
        return link is null ? NoContent() : Ok(ToResponse(link));
    }

    [HttpPost]
    [Authorize(Policy = "permission:validations.manage")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Link([FromBody] LinkGpfPersonRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var validatorId = GetCurrentUserId();
        if (validatorId is null)
        {
            return Unauthorized();
        }

        try
        {
            var link = await _gpfPersonLinkService.LinkAsync(request.UserId, request.CodUnico, validatorId.Value, cancellationToken);
            return CreatedAtAction(nameof(GetMine), null, ToResponse(link));
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    [HttpPost("{id:guid}/unlink")]
    [Authorize(Policy = "permission:validations.manage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Unlink(Guid id, CancellationToken cancellationToken)
    {
        var validatorId = GetCurrentUserId();
        if (validatorId is null)
        {
            return Unauthorized();
        }

        try
        {
            await _gpfPersonLinkService.UnlinkAsync(id, validatorId.Value, cancellationToken);
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

    private static GpfPersonLinkResponse ToResponse(Domain.Entities.GpfPersonLink link) => new()
    {
        Id = link.Id,
        UserId = link.UserId,
        UserName = link.User is null ? null : $"{link.User.Name} {link.User.LastName}",
        UserEmail = link.User?.Email,
        CodUnico = link.CodUnico,
        Status = link.Status.ToString(),
        LinkedAt = link.LinkedAt,
        LastQueriedAt = link.LastQueriedAt,
        ValidatedByUserId = link.ValidatedByUserId,
    };
}
