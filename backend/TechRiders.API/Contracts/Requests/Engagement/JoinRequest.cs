using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Engagement;

public sealed class JoinRequest
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string RequestType { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string CommunityRole { get; set; } = string.Empty;

    [StringLength(80)]
    public string? Audience { get; set; }

    [StringLength(200)]
    public string? Organization { get; set; }

    [StringLength(150)]
    public string? SessionTopic { get; set; }

    [StringLength(80)]
    public string? SessionFormat { get; set; }

    [Required]
    [StringLength(2000)]
    public string Motivation { get; set; } = string.Empty;
}