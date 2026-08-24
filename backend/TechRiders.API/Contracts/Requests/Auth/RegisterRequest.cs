using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Auth;

public sealed class RegisterRequest
{
    [Required]
    [StringLength(80, MinimumLength = 3)]
    public string Nickname { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}
