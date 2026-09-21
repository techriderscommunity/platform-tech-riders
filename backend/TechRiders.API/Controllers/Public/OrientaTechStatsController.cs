using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers.Public;

/// <summary>Count dinamico real de sesiones activas para la pagina Orienta Tech.</summary>
[ApiController]
[Route("api/public/orienta-tech/stats")]
[Produces("application/json")]
public sealed class OrientaTechStatsController : BaseApiController
{
    private readonly IPublicStatsService _publicStatsService;

    public OrientaTechStatsController(IPublicStatsService publicStatsService)
    {
        _publicStatsService = publicStatsService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrientaTechStatsResponse))]
    public async Task<ActionResult<OrientaTechStatsResponse>> Get(CancellationToken cancellationToken)
    {
        return Ok(await _publicStatsService.GetOrientaTechStatsAsync(cancellationToken));
    }
}
