using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRiders.Api.Contracts.Requests.Auth;
using TechRiders.Api.Contracts.Responses.Auth;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class AuthController : BaseApiController
{
    private readonly TechRidersDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthController(TechRidersDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
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
            var user = await DatabaseAuthService.RegisterAsync(
                _dbContext,
                request.Nickname,
                request.Name,
                request.LastName,
                request.Email,
                request.Password,
                cancellationToken);

            var profile = DatabaseAuthService.BuildUserProfile(user);
            var token = DatabaseAuthService.CreateToken(user, _configuration);

            return Ok(new RegisterResponse
            {
                Token = token,
                Message = "Cuenta creada correctamente.",
                Email = user.Email,
                User = new UserProfileResponse
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
                }
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

        var user = await DatabaseAuthService.AuthenticateAsync(_dbContext, request.Email, request.Password, cancellationToken);
        if (user is null)
        {
            return Unauthorized(new { message = "Credenciales inválidas." });
        }

        var profile = DatabaseAuthService.BuildUserProfile(user);
        var token = DatabaseAuthService.CreateToken(user, _configuration);

        return Ok(new LoginResponse
        {
            Token = token,
            User = new UserProfileResponse
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
            }
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

        var response = await DatabaseAuthService.RequestPasswordResetAsync(_dbContext, request.Email, cancellationToken);
        return Ok(response);
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

        var success = await DatabaseAuthService.ResetPasswordAsync(_dbContext, request.Email, request.Token, request.NewPassword, cancellationToken);
        if (!success)
        {
            return BadRequest(new { message = "El token es inválido o ha expirado." });
        }

        return Ok(new { message = "Contraseña actualizada correctamente." });
    }
}
