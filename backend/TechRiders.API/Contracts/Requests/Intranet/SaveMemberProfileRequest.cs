using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Intranet;

public sealed class SaveMemberProfileRequest
{
    [StringLength(254)]
    public string? UserKey { get; set; }

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Bio { get; set; } = string.Empty;

    [StringLength(500)]
    public string Interests { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Audience { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string CommunityRole { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Organization { get; set; }
}