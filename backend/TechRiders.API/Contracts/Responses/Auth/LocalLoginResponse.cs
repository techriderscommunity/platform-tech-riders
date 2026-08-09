namespace TechRiders.Api.Contracts.Responses.Auth;

public sealed class LocalLoginResponse
{
    public string Token { get; set; } = string.Empty;

    public LocalUserProfile User { get; set; } = new();
}