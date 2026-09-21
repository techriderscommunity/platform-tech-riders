using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers.Public;

/// <summary>Count dinamico real de ambassadors activos para la pagina Quienes somos.</summary>
[ApiController]
[Route("api/public/about/stats")]
[Produces("application/json")]
public sealed class AboutStatsController : BaseApiController
{
    private readonly IPublicStatsService _publicStatsService;

    public AboutStatsController(IPublicStatsService publicStatsService)
    {
        _publicStatsService = publicStatsService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AboutStatsResponse))]
    public async Task<ActionResult<AboutStatsResponse>> Get(CancellationToken cancellationToken)
    {
        return Ok(await _publicStatsService.GetAboutStatsAsync(cancellationToken));
    }
}
