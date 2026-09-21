using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers.Public;

/// <summary>Count dinamico real de ambassadors activos para la pagina Woman Tech.</summary>
[ApiController]
[Route("api/public/woman-tech/stats")]
[Produces("application/json")]
public sealed class WomanTechStatsController : BaseApiController
{
    private readonly IPublicStatsService _publicStatsService;

    public WomanTechStatsController(IPublicStatsService publicStatsService)
    {
        _publicStatsService = publicStatsService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WomanTechStatsResponse))]
    public async Task<ActionResult<WomanTechStatsResponse>> Get(CancellationToken cancellationToken)
    {
        return Ok(await _publicStatsService.GetWomanTechStatsAsync(cancellationToken));
    }
}
