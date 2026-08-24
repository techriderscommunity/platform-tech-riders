using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/intranet-operations")]
[Produces("application/json")]
public sealed class IntranetOperationsController : BaseApiController
{
    [HttpPost("trazas")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SaveTrace([FromBody] object request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Accepted(new { success = true, message = "Trace logging is deferred until the intranet domain model is finalized." });
    }
}
