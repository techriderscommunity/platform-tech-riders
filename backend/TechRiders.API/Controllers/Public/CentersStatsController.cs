using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers.Public;

/// <summary>Count dinamico real de centros activos.</summary>
[ApiController]
[Route("api/public/centers/stats")]
[Produces("application/json")]
public sealed class CentersStatsController : BaseApiController
{
    private readonly IPublicStatsService _publicStatsService;

    public CentersStatsController(IPublicStatsService publicStatsService)
    {
        _publicStatsService = publicStatsService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CentersStatsResponse))]
    public async Task<ActionResult<CentersStatsResponse>> Get(CancellationToken cancellationToken)
    {
        return Ok(await _publicStatsService.GetCentersStatsAsync(cancellationToken));
    }
}
