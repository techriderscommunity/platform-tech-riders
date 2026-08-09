using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests.Intranet;

public sealed class UpdateIntranetSettingRequest
{
    [Required]
    [StringLength(255)]
    public string Key { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Module { get; set; } = string.Empty;

    [Required]
    [StringLength(4000)]
    public string Value { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty;
}