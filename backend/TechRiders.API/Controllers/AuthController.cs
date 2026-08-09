using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Auth;
using TechRiders.Api.Contracts.Responses.Auth;
using TechRiders.Api.Services;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly ILocalAuthenticationService _localAuthenticationService;

    public AuthController(ILocalAuthenticationService localAuthenticationService)
    {
        _localAuthenticationService = localAuthenticationService ?? throw new ArgumentNullException(nameof(localAuthenticationService));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LocalLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<LocalLoginResponse> Login([FromBody] LocalLoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result = _localAuthenticationService.Authenticate(request);

        return result.StatusCode switch
        {
            StatusCodes.Status200OK => Ok(result.Response),
            StatusCodes.Status401Unauthorized => Unauthorized(new { message = result.Message }),
            StatusCodes.Status404NotFound => NotFound(new { message = result.Message }),
            StatusCodes.Status503ServiceUnavailable => StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                message = result.Message,
            }),
            _ => StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = "Local login failed.",
            }),
        };
    }
}
