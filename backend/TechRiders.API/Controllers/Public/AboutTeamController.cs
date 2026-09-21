using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Api.Controllers.Public;

/// <summary>Equipo real de la comunidad (Users agrupados por Capability) para la pagina Quienes somos.</summary>
[ApiController]
[Route("api/public/about/team")]
[Produces("application/json")]
public sealed class AboutTeamController : BaseApiController
{
    private readonly IPublicTeamService _publicTeamService;
    private readonly IProfileMediaService _profileMediaService;
    private readonly IUnitOfWork _unitOfWork;

    public AboutTeamController(IPublicTeamService publicTeamService, IProfileMediaService profileMediaService, IUnitOfWork unitOfWork)
    {
        _publicTeamService = publicTeamService;
        _profileMediaService = profileMediaService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<PublicTeamZoneResponse>))]
    public async Task<ActionResult<IReadOnlyList<PublicTeamZoneResponse>>> Get(CancellationToken cancellationToken)
    {
        return Ok(await _publicTeamService.GetTeamZonesAsync(cancellationToken));
    }

    [HttpGet("{userId:guid}/photo")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPhoto(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        var legacySlug = user is null ? null : $"{user.Name}-{user.LastName}";
        var content = await _profileMediaService.DownloadUserPhotoAsync(userId, legacySlug, cancellationToken);
        return content is null ? NotFound() : File(content.Content, content.ContentType);
    }
}
