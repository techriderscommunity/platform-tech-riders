using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Auth;

public sealed class ResetPasswordRequest
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(512)]
    public string Token { get; set; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 8)]
    public string NewPassword { get; set; } = string.Empty;
}
