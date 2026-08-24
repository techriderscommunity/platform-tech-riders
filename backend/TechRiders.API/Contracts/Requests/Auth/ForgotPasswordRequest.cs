using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Auth;

public sealed class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;
}
