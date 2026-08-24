using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TechRiders.Api.Contracts.Responses.Auth;
using TechRiders.Domain.Entities;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

public static class LocalAuthService
{
    private const int PBKDF2Iterations = 200_000;
    private const int KeyBytes = 32;

    public static async Task EnsureDefaultAdminAsync(TechRidersDbContext dbContext, IConfiguration configuration, ILogger logger, CancellationToken cancellationToken = default)
    {
        var localAuthSection = configuration.GetSection("LocalAuth");
        var email = localAuthSection["DefaultAdminEmail"] ?? "admin@techriders.local";
        var password = localAuthSection["DefaultAdminPassword"] ?? "TechRiders123!";
        var roleName = localAuthSection["DefaultAdminRole"] ?? "Admin";

        var role = await dbContext.Set<Role>()
            .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);

        if (role is null)
        {
            role = new Role
            {
                Id = Guid.NewGuid(),
                Name = roleName,
                Description = "Administrador local generado por la API"
            };

            await dbContext.Set<Role>().AddAsync(role, cancellationToken);
        }

        var user = await dbContext.Users
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
                PasswordHash = HashPassword(password),
                IsWorking = true,
                Phone = "+34600000000",
                Locality = "Madrid",
                About = "Cuenta de administración local para soporte técnico y validación de entorno",
            };

            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var alreadyHasRole = await dbContext.Set<UserRole>()
            .AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, cancellationToken);

        if (!alreadyHasRole)
        {
            dbContext.Set<UserRole>().Add(new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
            });
        }

        if (string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            user.PasswordHash = HashPassword(password);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Local auth admin user ensured for {Email}", email);
    }

    public static async Task<User> RegisterAsync(TechRidersDbContext dbContext, string nickname, string name, string lastName, string email, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Nickname, nombre y apellidos son obligatorios.");
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException("El correo electrónico no es válido.");
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");
        }

        var normalizedEmail = email.Trim();
        var existingUser = await dbContext.Users
            .AnyAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (existingUser)
        {
            throw new InvalidOperationException("Ya existe una cuenta registrada con ese correo.");
        }

        var ambassadorRole = await GetOrCreateRoleAsync(dbContext, "Ambassador", cancellationToken);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Nickname = nickname.Trim(),
            Name = name.Trim(),
            LastName = lastName.Trim(),
            Email = normalizedEmail,
            PasswordHash = HashPassword(password),
            IsWorking = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Phone = null,
            Locality = null,
            About = "Cuenta creada mediante registro local de TechRiders"
        };

        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        dbContext.Set<UserRole>().Add(new UserRole
        {
            UserId = user.Id,
            RoleId = ambassadorRole.Id,
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }

    public static async Task<User?> AuthenticateAsync(TechRidersDbContext dbContext, string email, string password, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();

        var user = await dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail && u.IsActive, cancellationToken);

        if (user is null) return null;
        if (string.IsNullOrWhiteSpace(user.PasswordHash)) return null;
        if (!VerifyPassword(password, user.PasswordHash)) return null;

        return user;
    }

    public static async Task<ForgotPasswordResponse> RequestPasswordResetAsync(TechRidersDbContext dbContext, string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail && u.IsActive, cancellationToken);

        if (user is null)
        {
            return new ForgotPasswordResponse
            {
                Success = false,
                Message = "Si la cuenta existe, recibirás el enlace de recuperación en tu correo.",
                Token = string.Empty
            };
        }

        var resetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        user.PasswordResetToken = resetToken;
        user.PasswordResetTokenExpiresAt = DateTime.UtcNow.AddMinutes(15);
        user.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new ForgotPasswordResponse
        {
            Success = true,
            Message = "Si la cuenta existe, recibirás el enlace de recuperación en tu correo.",
            Token = resetToken
        };
    }

    public static async Task<bool> ResetPasswordAsync(TechRidersDbContext dbContext, string email, string token, string newPassword, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();
        var normalizedToken = token.Trim();

        if (string.IsNullOrWhiteSpace(normalizedToken) || string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
        {
            return false;
        }

        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail && u.IsActive, cancellationToken);

        if (user is null) return false;
        if (string.IsNullOrWhiteSpace(user.PasswordResetToken) || !string.Equals(user.PasswordResetToken, normalizedToken, StringComparison.Ordinal))
        {
            return false;
        }

        if (user.PasswordResetTokenExpiresAt is null || user.PasswordResetTokenExpiresAt.Value < DateTime.UtcNow)
        {
            return false;
        }

        user.PasswordHash = HashPassword(newPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiresAt = null;
        user.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public static string CreateToken(User user, IConfiguration configuration)
    {
        var localAuthSettings = configuration.GetSection("LocalAuth");
        var issuer = localAuthSettings["Issuer"] ?? "TechRidersLocalAuth";
        var audience = localAuthSettings["Audience"] ?? "TechRidersApi";
        var signingKey = localAuthSettings["SigningKey"] ?? "techriders-local-auth-signing-key-2025";
        var lifetimeHours = localAuthSettings.GetValue<int?>("TokenLifetimeHours") ?? 8;

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

    public static LocalUserProfile BuildUserProfile(User user)
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

        return new LocalUserProfile
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            Name = user.Name + " " + user.LastName,
            Role = roles[0],
            Roles = roles,
        };
    }

    public static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = new Rfc2898DeriveBytes(password, salt, PBKDF2Iterations, HashAlgorithmName.SHA256).GetBytes(KeyBytes);
        var payload = Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        return payload;
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
        {
            return false;
        }

        var separatorIndex = storedHash.IndexOf(':');
        if (separatorIndex <= 0 || separatorIndex == storedHash.Length - 1)
        {
            return false;
        }

        try
        {
            var salt = Convert.FromBase64String(storedHash[..separatorIndex]);
            var expectedHash = Convert.FromBase64String(storedHash[(separatorIndex + 1)..]);
            var actualHash = new Rfc2898DeriveBytes(password, salt, PBKDF2Iterations, HashAlgorithmName.SHA256).GetBytes(expectedHash.Length);
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static async Task<Role> GetOrCreateRoleAsync(TechRidersDbContext dbContext, string roleName, CancellationToken cancellationToken)
    {
        var normalizedRoleName = roleName.Trim();
        var existing = await dbContext.Set<Role>()
            .FirstOrDefaultAsync(r => r.Name == normalizedRoleName, cancellationToken);

        if (existing is not null)
        {
            return existing;
        }

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = normalizedRoleName,
            Description = "Rol generado por el flujo local de autenticación.",
            CreatedAt = DateTime.UtcNow,
        };

        await dbContext.Set<Role>().AddAsync(role, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return role;
    }

    private static string NormalizeRole(string role)
    {
        var normalized = role.Trim();
        if (normalized.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase)) return "superadmin";
        if (normalized.Equals("Admin", StringComparison.OrdinalIgnoreCase)) return "admin";
        if (normalized.Equals("Staff", StringComparison.OrdinalIgnoreCase)) return "staff";
        if (normalized.Equals("Coordinador", StringComparison.OrdinalIgnoreCase)) return "coordinador";
        if (normalized.Equals("Empresa", StringComparison.OrdinalIgnoreCase)) return "empresa";
        if (normalized.Equals("Junior", StringComparison.OrdinalIgnoreCase)) return "junior";
        if (normalized.Equals("Colaborador", StringComparison.OrdinalIgnoreCase)) return "colaborador";
        if (normalized.Equals("Ambassador", StringComparison.OrdinalIgnoreCase) || normalized.Equals("Embajador", StringComparison.OrdinalIgnoreCase)) return "embajador";
        if (normalized.Equals("Member", StringComparison.OrdinalIgnoreCase)) return "member";
        return normalized.ToLowerInvariant();
    }
}
