using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Engagement;

public sealed class ContactRequest
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;
}