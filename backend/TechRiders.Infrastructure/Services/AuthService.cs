using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TechRiders.Application.Interfaces;
using TechRiders.Application.Social;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>Registro, login y recuperacion de contrasena contra usuarios reales de BD.</summary>
public sealed class AuthService : IAuthService
{
    private readonly TechRidersDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<AuthService> _logger;

    public AuthService(TechRidersDbContext dbContext, IConfiguration configuration, IPasswordHasher passwordHasher, ILogger<AuthService> logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task EnsureDefaultAdminAsync(CancellationToken cancellationToken = default)
    {
        var authSection = GetAuthSection();
        var email = authSection["DefaultAdminEmail"] ?? "admin@techriders.local";
        var password = authSection["DefaultAdminPassword"];
        var roleName = authSection["DefaultAdminRole"] ?? "Admin";

        if (string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Default admin seed skipped because Auth:DefaultAdminPassword is not configured.");
            return;
        }

        var role = await _dbContext.Set<Role>().FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
        if (role is null)
        {
            role = new Role { Id = Guid.NewGuid(), Name = roleName, Description = "Administrador generado por la API" };
            await _dbContext.Set<Role>().AddAsync(role, cancellationToken);
        }

        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Nickname = "admin",
                Name = "Admin",
                LastName = "TechRiders",
                Email = email,
                PasswordHash = _passwordHasher.HashPassword(password),
                IsWorking = true,
                Phone = "+34600000000",
                Locality = "Madrid",
                About = "Cuenta de administracion para soporte tecnico y validacion de entorno",
            };

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var alreadyHasRole = await _dbContext.Set<UserRole>().AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, cancellationToken);
        if (!alreadyHasRole)
        {
            _dbContext.Set<UserRole>().Add(new UserRole { UserId = user.Id, RoleId = role.Id });
        }

        if (string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            user.PasswordHash = _passwordHasher.HashPassword(password);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        var hasMembership = await _dbContext.Set<Membership>().AnyAsync(m => m.UserId == user.Id, cancellationToken);
        if (!hasMembership)
        {
            var staffProfile = await IdentityCatalogSeedService.GetProfileAsync(_dbContext, IdentityCatalogSeedService.StaffTajamarProfile, cancellationToken);
            _dbContext.Set<UserProfileHistory>().Add(new UserProfileHistory { UserId = user.Id, ProfileId = staffProfile.Id, IsCurrent = true });
            _dbContext.Set<Membership>().Add(new Membership { UserId = user.Id, Status = MembershipStatus.Activa, ActivatedAt = DateTime.UtcNow, Origin = "seed-admin" });
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        _logger.LogInformation("Database auth admin user ensured for {Email}", email);
    }

    public async Task<User> RegisterAsync(string nickname, string name, string lastName, string email, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Nickname, nombre y apellidos son obligatorios.");
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException("El correo electronico no es valido.");
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            throw new ArgumentException("La contrasena debe tener al menos 8 caracteres.");
        }

        var normalizedEmail = email.Trim();
        var existingUser = await _dbContext.Users.AnyAsync(u => u.Email == normalizedEmail, cancellationToken);
        if (existingUser)
        {
            throw new InvalidOperationException("Ya existe una cuenta registrada con ese correo.");
        }

        var memberRole = await GetOrCreateRoleAsync("Member", cancellationToken);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Nickname = nickname.Trim(),
            Name = name.Trim(),
            LastName = lastName.Trim(),
            Email = normalizedEmail,
            PasswordHash = _passwordHasher.HashPassword(password),
            IsWorking = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Phone = null,
            Locality = null,
            About = "Cuenta creada mediante registro de TechRiders"
        };

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _dbContext.Set<UserRole>().Add(new UserRole { UserId = user.Id, RoleId = memberRole.Id });

        var visitanteProfile = await IdentityCatalogSeedService.GetProfileAsync(_dbContext, IdentityCatalogSeedService.VisitanteProfile, cancellationToken);
        _dbContext.Set<UserProfileHistory>().Add(new UserProfileHistory { UserId = user.Id, ProfileId = visitanteProfile.Id, IsCurrent = true });
        _dbContext.Set<Membership>().Add(new Membership { UserId = user.Id, Status = MembershipStatus.Activa, ActivatedAt = DateTime.UtcNow, Origin = "auto-registro" });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<User?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();

        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(role => role.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail && u.IsActive, cancellationToken);

        if (user is null) return null;
        if (string.IsNullOrWhiteSpace(user.PasswordHash)) return null;
        if (!_passwordHasher.VerifyPassword(password, user.PasswordHash)) return null;

        return user;
    }

    public async Task<PasswordResetResult> RequestPasswordResetAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail && u.IsActive, cancellationToken);

        const string message = "Si la cuenta existe, recibiras el enlace de recuperacion en tu correo.";
        if (user is null)
        {
            return new PasswordResetResult(false, message, string.Empty);
        }

        var resetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        user.PasswordResetToken = resetToken;
        user.PasswordResetTokenExpiresAt = DateTime.UtcNow.AddMinutes(15);
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new PasswordResetResult(true, message, resetToken);
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();
        var normalizedToken = token.Trim();

        if (string.IsNullOrWhiteSpace(normalizedToken) || string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
        {
            return false;
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail && u.IsActive, cancellationToken);
        if (user is null) return false;
        if (string.IsNullOrWhiteSpace(user.PasswordResetToken) || !string.Equals(user.PasswordResetToken, normalizedToken, StringComparison.Ordinal))
        {
            return false;
        }

        if (user.PasswordResetTokenExpiresAt is null || user.PasswordResetTokenExpiresAt.Value < DateTime.UtcNow)
        {
            return false;
        }

        user.PasswordHash = _passwordHasher.HashPassword(newPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiresAt = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public string CreateToken(User user)
    {
        var authSettings = GetAuthSection();
        var issuer = authSettings["Issuer"] ?? "TechRidersAuth";
        var audience = authSettings["Audience"] ?? "TechRidersApi";
        var signingKey = authSettings["SigningKey"];
        var lifetimeHours = authSettings.GetValue<int?>("TokenLifetimeHours") ?? 8;

        if (string.IsNullOrWhiteSpace(signingKey))
        {
            throw new InvalidOperationException("Auth:SigningKey must be configured.");
        }

        var roles = user.UserRoles
            .Select(ur => ur.Role.Name)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .DefaultIfEmpty("Admin")
            .ToArray();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name + " " + user.LastName),
            new(ClaimTypes.Email, user.Email),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var permissions = user.UserRoles
            .SelectMany(userRole => userRole.Role.RolePermissions)
            .Select(rolePermission => rolePermission.Permission.Name)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct(StringComparer.OrdinalIgnoreCase);
        claims.AddRange(permissions.Select(permission => new Claim("permission", permission)));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(lifetimeHours),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public AuthUserProfile BuildUserProfile(User user)
    {
        var roles = user.UserRoles
            .Select(ur => ur.Role.Name)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(NormalizeRole)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (roles.Length == 0)
        {
            roles = ["admin"];
        }

        return new AuthUserProfile(
            user.Id.ToString(),
            user.Email,
            user.Name + " " + user.LastName,
            roles[0],
            roles,
            SocialProfileUrls.Build(SocialProfileUrls.LinkedIn, user.LinkedIn),
            SocialProfileUrls.Build(SocialProfileUrls.Instagram, user.Instagram),
            SocialProfileUrls.Build(SocialProfileUrls.X, user.X),
            SocialProfileUrls.Build(SocialProfileUrls.YouTube, user.YouTube),
            SocialProfileUrls.Build(SocialProfileUrls.GitHub, user.Github));
    }

    private async Task<Role> GetOrCreateRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        var normalizedRoleName = roleName.Trim();
        var existing = await _dbContext.Set<Role>().FirstOrDefaultAsync(r => r.Name == normalizedRoleName, cancellationToken);
        if (existing is not null)
        {
            return existing;
        }

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = normalizedRoleName,
            Description = "Rol generado por el flujo de autenticacion.",
            CreatedAt = DateTime.UtcNow,
        };

        await _dbContext.Set<Role>().AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return role;
    }

    private IConfigurationSection GetAuthSection() => _configuration.GetSection("Auth");

    private static string NormalizeRole(string role)
    {
        var normalized = role.Trim();
        if (normalized.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase) || normalized.Equals("Admin", StringComparison.OrdinalIgnoreCase)) return "admin";
        if (normalized.Equals("Staff", StringComparison.OrdinalIgnoreCase)) return "staff";
        if (normalized.Equals("Community Leader", StringComparison.OrdinalIgnoreCase) || normalized.Equals("CommunityLeader", StringComparison.OrdinalIgnoreCase) || normalized.Equals("Coordinador", StringComparison.OrdinalIgnoreCase) || normalized.Equals("Colaborador", StringComparison.OrdinalIgnoreCase)) return "community-leader";
        if (normalized.Equals("Ambassador", StringComparison.OrdinalIgnoreCase) || normalized.Equals("Embajador", StringComparison.OrdinalIgnoreCase)) return "ambassador";
        if (normalized.Equals("Center", StringComparison.OrdinalIgnoreCase) || normalized.Equals("Centro", StringComparison.OrdinalIgnoreCase)) return "center";
        if (normalized.Equals("Community Partner", StringComparison.OrdinalIgnoreCase) || normalized.Equals("CommunityPartner", StringComparison.OrdinalIgnoreCase)) return "community-partner";
        if (normalized.Equals("Member", StringComparison.OrdinalIgnoreCase) || normalized.Equals("Junior", StringComparison.OrdinalIgnoreCase) || normalized.Equals("Empresa", StringComparison.OrdinalIgnoreCase)) return "member";
        return normalized.ToLowerInvariant();
    }
}
