using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Intranet;

public sealed class SaveAmbassadorPortalRequest
{
    [StringLength(254)]
    public string? UserKey { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Bio { get; set; } = string.Empty;

    [StringLength(500)]
    public string Specialties { get; set; } = string.Empty;

    [StringLength(500)]
    public string Availability { get; set; } = string.Empty;
}