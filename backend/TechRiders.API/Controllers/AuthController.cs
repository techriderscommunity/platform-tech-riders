using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechRiders.Api.Services;

namespace TechRiders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly LocalAuthOptions localAuthOptions;
    private readonly IHostEnvironment hostEnvironment;
    private readonly IConfiguration configuration;

    public AuthController(
        IOptions<LocalAuthOptions> localAuthOptions,
        IHostEnvironment hostEnvironment,
        IConfiguration configuration)
    {
        this.localAuthOptions = localAuthOptions.Value;
        this.hostEnvironment = hostEnvironment;
        this.configuration = configuration;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LocalLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<LocalLoginResponse> Login([FromBody] LocalLoginRequest request)
    {
        if (!hostEnvironment.IsDevelopment() || !localAuthOptions.Enabled)
        {
            return NotFound(new { message = "Local login is disabled." });
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var users = ResolveUsers();
        if (users.Count == 0)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                message = "Local auth users are not configured. Set LocalAuth:Users or LOCAL_AUTH_USERS_JSON."
            });
        }

        var user = users.FirstOrDefault(item =>
            string.Equals(item.Email, request.Email, StringComparison.OrdinalIgnoreCase)
            && string.Equals(item.Password, request.Password, StringComparison.Ordinal));

        if (user is null)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        var primaryRole = string.IsNullOrWhiteSpace(user.Role) ? "junior" : user.Role.Trim().ToLowerInvariant();
        var functionalRoles = user.Roles
            .Select(role => role.Trim().ToLowerInvariant())
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Append(primaryRole)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var roleClaims = BuildRoleClaims(functionalRoles);
        var token = BuildJwt(user, roleClaims);

        return Ok(new LocalLoginResponse
        {
            Token = token,
            User = new LocalUserProfile
            {
                Id = user.Email.Trim().ToLowerInvariant(),
                Email = user.Email.Trim().ToLowerInvariant(),
                Name = string.IsNullOrWhiteSpace(user.Name) ? user.Email : user.Name,
                Role = primaryRole,
                Roles = functionalRoles,
            },
        });
    }

    private IReadOnlyList<LocalAuthUser> ResolveUsers()
    {
        var rawJsonUsers = configuration["LOCAL_AUTH_USERS_JSON"];
        if (!string.IsNullOrWhiteSpace(rawJsonUsers))
        {
            try
            {
                var parsed = JsonSerializer.Deserialize<List<LocalAuthUser>>(rawJsonUsers, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                if (parsed is { Count: > 0 })
                {
                    return parsed;
                }
            }
            catch
            {
                // Si el JSON de entorno es inválido, se intentará con la configuración tipada.
            }
        }

        return localAuthOptions.Users;
    }

    private string BuildJwt(LocalAuthUser user, IReadOnlyCollection<string> roleClaims)
    {
        var issuer = LocalAuthOptions.DefaultIssuer;
        var audience = LocalAuthOptions.DefaultAudience;
        var lifetime = localAuthOptions.TokenLifetimeHours <= 0 ? 8 : localAuthOptions.TokenLifetimeHours;
        var jwtKey = string.IsNullOrWhiteSpace(localAuthOptions.JwtKey)
            ? configuration["JWT_KEY"]
            : localAuthOptions.JwtKey;

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? throw new InvalidOperationException("JWT key for local auth is not configured.")));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email.Trim().ToLowerInvariant()),
            new(JwtRegisteredClaimNames.Email, user.Email.Trim().ToLowerInvariant()),
            new(ClaimTypes.NameIdentifier, user.Email.Trim().ToLowerInvariant()),
            new(ClaimTypes.Name, string.IsNullOrWhiteSpace(user.Name) ? user.Email : user.Name),
        };

        claims.AddRange(roleClaims.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(lifetime),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static IReadOnlyCollection<string> BuildRoleClaims(IReadOnlyCollection<string> roles)
    {
        var claimSet = new HashSet<string>(roles, StringComparer.OrdinalIgnoreCase);

        if (claimSet.Contains("admin") || claimSet.Contains("superadmin"))
        {
            claimSet.Add("Admin");
        }

        if (claimSet.Contains("staff") || claimSet.Contains("coordinador") || claimSet.Contains("admin") || claimSet.Contains("superadmin"))
        {
            claimSet.Add("Manager");
        }

        return claimSet;
    }
}

public sealed class LocalLoginRequest
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(128)]
    public string Password { get; set; } = string.Empty;
}

public sealed class LocalLoginResponse
{
    public string Token { get; set; } = string.Empty;

    public LocalUserProfile User { get; set; } = new();
}

public sealed class LocalUserProfile
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public IReadOnlyCollection<string> Roles { get; set; } = [];
}
