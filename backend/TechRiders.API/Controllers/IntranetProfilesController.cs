using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Intranet;
using TechRiders.Api.Services;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/intranet")]
[Produces("application/json")]
public sealed class IntranetProfilesController : ControllerBase
{
    private readonly IMvpRuntimeStateStore _mvpRuntimeStateStore;

    public IntranetProfilesController(IMvpRuntimeStateStore mvpRuntimeStateStore)
    {
        _mvpRuntimeStateStore = mvpRuntimeStateStore ?? throw new ArgumentNullException(nameof(mvpRuntimeStateStore));
    }

    [HttpGet("perfil")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MemberProfileState))]
    public IActionResult GetMemberProfile([FromQuery] string? userKey, [FromQuery] string? email)
    {
        var profile = _mvpRuntimeStateStore.GetOrCreateMemberProfile(userKey ?? email ?? string.Empty, email);
        return Ok(profile);
    }

    [HttpPut("perfil")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MemberProfileState))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveMemberProfile([FromBody] SaveMemberProfileRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var profile = new MemberProfileState
        {
            Name = request.Name,
            Email = request.Email,
            Bio = request.Bio,
            Interests = request.Interests,
            Audience = request.Audience,
            CommunityRole = request.CommunityRole,
            Organization = request.Organization ?? string.Empty,
        };

        _mvpRuntimeStateStore.UpsertMemberProfile(request.UserKey ?? request.Email, profile);
        return Ok(profile);
    }

    [HttpGet("ambassador-profile")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AmbassadorPortalState))]
    public IActionResult GetAmbassadorPortal([FromQuery] string? userKey, [FromQuery] string? email)
    {
        var profile = _mvpRuntimeStateStore.GetOrCreateAmbassadorPortal(userKey ?? email ?? string.Empty, email);
        return Ok(profile);
    }

    [HttpPut("ambassador-profile")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AmbassadorPortalState))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveAmbassadorPortal([FromBody] SaveAmbassadorPortalRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var profile = new AmbassadorPortalState
        {
            Email = request.Email,
            Bio = request.Bio,
            Specialties = request.Specialties,
            Availability = request.Availability,
        };

        _mvpRuntimeStateStore.UpsertAmbassadorPortal(request.UserKey ?? request.Email, profile);
        return Ok(profile);
    }

    [HttpGet("mis-categorias")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
    public IActionResult GetCurrentUserCategories([FromQuery] string? userKey)
    {
        var categories = _mvpRuntimeStateStore.GetUserCategories(userKey ?? string.Empty);
        return Ok(categories);
    }

    [HttpPut("mis-categorias")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveCurrentUserCategories([FromBody] SaveCategoriesRequest request)
    {
        if (request.Categories.Count == 0)
        {
            return BadRequest(new { error = "At least one category is required." });
        }

        _mvpRuntimeStateStore.UpsertUserCategories(request.UserKey ?? string.Empty, request.Categories);
        return Ok(request.Categories);
    }
}