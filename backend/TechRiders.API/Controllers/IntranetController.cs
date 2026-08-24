using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class IntranetController : BaseApiController
{
    [HttpGet("perfil")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetMemberProfile([FromQuery] string? userKey, [FromQuery] string? email)
    {
        return Ok(new { userKey, email, message = "Profile persistence is not implemented yet in the current domain model." });
    }

    [HttpPut("perfil")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveMemberProfile([FromBody] object request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(new { success = true, message = "Profile persistence handling is deferred until the intranet domain model is finalized." });
    }

    [HttpGet("ambassador-profile")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAmbassadorPortal([FromQuery] string? userKey, [FromQuery] string? email)
    {
        return Ok(new { userKey, email, message = "Ambassador profile persistence is not implemented yet in the current domain model." });
    }

    [HttpPut("ambassador-profile")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveAmbassadorPortal([FromBody] object request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(new { success = true, message = "Ambassador profile persistence handling is deferred until the intranet domain model is finalized." });
    }
}
