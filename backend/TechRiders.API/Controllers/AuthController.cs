using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Auth;
using TechRiders.Api.Contracts.Responses.Auth;
using TechRiders.Application.Interfaces;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var user = await _authService.RegisterAsync(
                request.Nickname,
                request.Name,
                request.LastName,
                request.Email,
                request.Password,
                cancellationToken);

            var profile = _authService.BuildUserProfile(user);
            var token = _authService.CreateToken(user);

            return Ok(new RegisterResponse
            {
                Token = token,
                Message = "Cuenta creada correctamente.",
                Email = user.Email,
                User = ToUserProfileResponse(profile)
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var user = await _authService.AuthenticateAsync(request.Email, request.Password, cancellationToken);
        if (user is null)
        {
            return Unauthorized(new { message = "Credenciales inválidas." });
        }

        var profile = _authService.BuildUserProfile(user);
        var token = _authService.CreateToken(user);

        return Ok(new LoginResponse
        {
            Token = token,
            User = ToUserProfileResponse(profile)
        });
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result = await _authService.RequestPasswordResetAsync(request.Email, cancellationToken);
        return Ok(new ForgotPasswordResponse { Success = result.Success, Message = result.Message, Token = result.Token });
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var success = await _authService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword, cancellationToken);
        if (!success)
        {
            return BadRequest(new { message = "El token es inválido o ha expirado." });
        }

        return Ok(new { message = "Contraseña actualizada correctamente." });
    }

    private static UserProfileResponse ToUserProfileResponse(AuthUserProfile profile) => new()
    {
        Id = profile.Id,
        Email = profile.Email,
        Name = profile.Name,
        Role = profile.Role,
        Roles = profile.Roles,
        LinkedIn = profile.LinkedIn,
        Instagram = profile.Instagram,
        X = profile.X,
        YouTube = profile.YouTube,
        Github = profile.Github
    };
}
