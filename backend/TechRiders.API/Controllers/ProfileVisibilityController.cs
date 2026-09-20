using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Visibility;
using TechRiders.Api.Contracts.Responses.Visibility;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

/// <summary>Visibilidad de campos del perfil (Requisitos Arquitectura §10). Por defecto Private hasta configuración explícita.</summary>
[ApiController]
[Route("api/profile-visibility")]
[Produces("application/json")]
[Authorize]
public sealed class ProfileVisibilityController : BaseApiController
{
    private readonly IProfileVisibilityService _profileVisibilityService;

    public ProfileVisibilityController(IProfileVisibilityService profileVisibilityService)
    {
        _profileVisibilityService = profileVisibilityService;
    }

    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FieldVisibilityResponse>))]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var entries = await _profileVisibilityService.GetForUserAsync(userId.Value, cancellationToken);
        return Ok(entries.Select(e => new FieldVisibilityResponse { FieldKey = e.FieldKey, Visibility = e.Visibility.ToString() }));
    }

    [HttpPut("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FieldVisibilityResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetMine([FromBody] SetFieldVisibilityRequest request, CancellationToken cancellationToken)
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
            var entry = await _profileVisibilityService.SetAsync(userId.Value, request.FieldKey, request.Visibility, cancellationToken);
            return Ok(new FieldVisibilityResponse { FieldKey = entry.FieldKey, Visibility = entry.Visibility.ToString() });
        }
        catch (ArgumentException ex)
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
