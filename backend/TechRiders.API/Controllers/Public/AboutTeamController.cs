using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers.Public;

/// <summary>Equipo real de la comunidad (Users agrupados por Capability) para la pagina Quienes somos.</summary>
[ApiController]
[Route("api/public/about/team")]
[Produces("application/json")]
public sealed class AboutTeamController : BaseApiController
{
    private readonly IPublicTeamService _publicTeamService;

    public AboutTeamController(IPublicTeamService publicTeamService)
    {
        _publicTeamService = publicTeamService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<PublicTeamZoneResponse>))]
    public async Task<ActionResult<IReadOnlyList<PublicTeamZoneResponse>>> Get(CancellationToken cancellationToken)
    {
        return Ok(await _publicTeamService.GetTeamZonesAsync(cancellationToken));
    }
}
