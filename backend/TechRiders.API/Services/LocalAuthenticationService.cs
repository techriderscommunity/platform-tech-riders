using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TechRiders.Api.Contracts.Requests.Auth;
using TechRiders.Api.Contracts.Responses.Auth;

namespace TechRiders.Api.Services;

public interface ILocalAuthenticationService
{
    LocalAuthenticationResult Authenticate(LocalLoginRequest request);
}

public sealed class LocalAuthenticationService : ILocalAuthenticationService
{
    private readonly LocalAuthOptions _localAuthOptions;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IConfiguration _configuration;

    public LocalAuthenticationService(
        IOptions<LocalAuthOptions> localAuthOptions,
        IHostEnvironment hostEnvironment,
        IConfiguration configuration)
    {
        _localAuthOptions = localAuthOptions.Value;
        _hostEnvironment = hostEnvironment;
        _configuration = configuration;
    }

    public LocalAuthenticationResult Authenticate(LocalLoginRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!_hostEnvironment.IsDevelopment() || !_localAuthOptions.Enabled)
        {
            return LocalAuthenticationResult.NotFound("Local login is disabled.");
        }

        var users = ResolveUsers();
        if (users.Count == 0)
        {
            return LocalAuthenticationResult.ServiceUnavailable(
                "Local auth users are not configured. Set LocalAuth:Users or LOCAL_AUTH_USERS_JSON.");
        }

        var user = users.FirstOrDefault(item =>
            string.Equals(item.Email, request.Email, StringComparison.OrdinalIgnoreCase)
            && string.Equals(item.Password, request.Password, StringComparison.Ordinal));

        if (user is null)
        {
            return LocalAuthenticationResult.Unauthorized("Invalid credentials.");
        }

        var primaryRole = string.IsNullOrWhiteSpace(user.Role) ? "junior" : user.Role.Trim().ToLowerInvariant();
        var functionalRoles = user.Roles
            .Select(role => role.Trim().ToLowerInvariant())
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Append(primaryRole)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var roleClaims = BuildRoleClaims(functionalRoles);
        string token;

        try
        {
            token = BuildJwt(user, roleClaims);
        }
        catch (InvalidOperationException ex)
        {
            return LocalAuthenticationResult.ServiceUnavailable(ex.Message);
        }

        return LocalAuthenticationResult.Success(new LocalLoginResponse
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
        var rawJsonUsers = _configuration["LOCAL_AUTH_USERS_JSON"];
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
            }
        }

        return _localAuthOptions.Users;
    }

    private string BuildJwt(LocalAuthUser user, IReadOnlyCollection<string> roleClaims)
    {
        var issuer = LocalAuthOptions.DefaultIssuer;
        var audience = LocalAuthOptions.DefaultAudience;
        var lifetime = _localAuthOptions.TokenLifetimeHours <= 0 ? 8 : _localAuthOptions.TokenLifetimeHours;
        var jwtKey = string.IsNullOrWhiteSpace(_localAuthOptions.JwtKey)
            ? _configuration["JWT_KEY"]
            : _localAuthOptions.JwtKey;

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException("Local auth JWT signing key is not configured.");
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
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

public sealed class LocalAuthenticationResult
{
    public int StatusCode { get; init; }

    public string? Message { get; init; }

    public LocalLoginResponse? Response { get; init; }

    public static LocalAuthenticationResult Success(LocalLoginResponse response) => new()
    {
        StatusCode = StatusCodes.Status200OK,
        Response = response,
    };

    public static LocalAuthenticationResult Unauthorized(string message) => new()
    {
        StatusCode = StatusCodes.Status401Unauthorized,
        Message = message,
    };

    public static LocalAuthenticationResult NotFound(string message) => new()
    {
        StatusCode = StatusCodes.Status404NotFound,
        Message = message,
    };

    public static LocalAuthenticationResult ServiceUnavailable(string message) => new()
    {
        StatusCode = StatusCodes.Status503ServiceUnavailable,
        Message = message,
    };
}