using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Preferences;
using TechRiders.Api.Contracts.Responses.Preferences;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

/// <summary>Catálogo de taxonomía y preferencias de la persona (Requisitos Arquitectura §5).</summary>
[ApiController]
[Route("api/preferences")]
[Produces("application/json")]
[Authorize]
public sealed class PreferencesController : BaseApiController
{
    private readonly TechRidersDbContext _dbContext;

    public PreferencesController(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("catalog")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PreferenceDimensionResponse>))]
    public async Task<IActionResult> GetCatalog(CancellationToken cancellationToken)
    {
        var dimensions = await PreferenceService.GetCatalogAsync(_dbContext, cancellationToken);
        return Ok(dimensions.Select(d => new PreferenceDimensionResponse
        {
            Id = d.Id,
            Code = d.Code,
            Name = d.Name,
            Values = d.Values.Select(v => new PreferenceDimensionValueResponse
            {
                Id = v.Id,
                Code = v.Code,
                Name = v.Name,
                ParentValueId = v.ParentValueId,
            }).ToList(),
        }));
    }

    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Guid>))]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var values = await PreferenceService.GetUserPreferenceValueIdsAsync(_dbContext, userId.Value, cancellationToken);
        return Ok(values);
    }

    [HttpPut("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetMine([FromBody] SetUserPreferencesRequest request, CancellationToken cancellationToken)
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

        await PreferenceService.SetUserPreferencesAsync(_dbContext, userId.Value, request.DimensionValueIds, cancellationToken);
        return Ok();
    }

    private Guid? GetCurrentUserId()
    {
        var raw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(raw, out var id) ? id : null;
    }
}
