namespace TechRiders.Api.Contracts.Responses.Auth;

public sealed class LocalUserProfile
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public IReadOnlyCollection<string> Roles { get; set; } = [];
}