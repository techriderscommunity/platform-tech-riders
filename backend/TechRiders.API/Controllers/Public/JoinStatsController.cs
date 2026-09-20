using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers.Public;

/// <summary>Count dinamico real de ambassadors activos para la pagina de alta (Unete).</summary>
[ApiController]
[Route("api/public/join/stats")]
[Produces("application/json")]
public sealed class JoinStatsController : BaseApiController
{
    private readonly IPublicStatsService _publicStatsService;

    public JoinStatsController(IPublicStatsService publicStatsService)
    {
        _publicStatsService = publicStatsService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JoinStatsResponse))]
    public async Task<ActionResult<JoinStatsResponse>> Get(CancellationToken cancellationToken)
    {
        return Ok(await _publicStatsService.GetJoinStatsAsync(cancellationToken));
    }
}
