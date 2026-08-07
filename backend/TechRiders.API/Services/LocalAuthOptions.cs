namespace TechRiders.Api.Services;

public sealed class LocalAuthOptions
{
    public const string SectionName = "LocalAuth";

    public const string DefaultIssuer = "techriders-local-auth";

    public const string DefaultAudience = "techriders-local-clients";

    public bool Enabled { get; set; } = true;

    public string JwtKey { get; set; } = string.Empty;

    public int TokenLifetimeHours { get; set; } = 8;

    public List<LocalAuthUser> Users { get; set; } = [];
}

public sealed class LocalAuthUser
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = "junior";

    public List<string> Roles { get; set; } = [];
}
