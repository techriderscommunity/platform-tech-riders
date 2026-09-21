namespace TechRiders.Api.Contracts.Responses.Auth;

public sealed class RegisterResponse
{
    public string Token { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public UserProfileResponse User { get; set; } = new();
}
