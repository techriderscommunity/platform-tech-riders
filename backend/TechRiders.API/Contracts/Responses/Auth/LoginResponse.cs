namespace TechRiders.Api.Contracts.Responses.Auth;

public sealed class LoginResponse
{
    public string Token { get; set; } = string.Empty;

    public UserProfileResponse User { get; set; } = new();
}