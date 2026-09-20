using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers.Public;

/// <summary>Counts dinamicos reales para la home publica (sin contenido estatico).</summary>
[ApiController]
[Route("api/public/home/stats")]
[Produces("application/json")]
public sealed class HomeStatsController : BaseApiController
{
    private readonly IPublicStatsService _publicStatsService;

    public HomeStatsController(IPublicStatsService publicStatsService)
    {
        _publicStatsService = publicStatsService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomeStatsResponse))]
    public async Task<ActionResult<HomeStatsResponse>> Get(CancellationToken cancellationToken)
    {
        return Ok(await _publicStatsService.GetHomeStatsAsync(cancellationToken));
    }
}
