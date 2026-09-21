namespace TechRiders.Api.Contracts.Responses.Auth;

public sealed class UserProfileResponse
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public IReadOnlyCollection<string> Roles { get; set; } = [];

    public string? LinkedIn { get; set; }

    public string? Instagram { get; set; }

    public string? X { get; set; }

    public string? YouTube { get; set; }

    public string? Github { get; set; }
}