using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/intranet-profiles")]
[Produces("application/json")]
public sealed class IntranetProfilesController : BaseApiController
{
    [HttpGet("perfil")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetMemberProfile([FromQuery] string? userKey, [FromQuery] string? email)
    {
        return Ok(new { userKey, email, message = "Profile retrieval is deferred until the intranet domain model is finalized." });
    }
}
