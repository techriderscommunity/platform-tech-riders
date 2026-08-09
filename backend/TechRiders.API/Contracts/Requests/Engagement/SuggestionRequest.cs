using System.ComponentModel.DataAnnotations;

namespace TechRiders.Api.Contracts.Requests.Engagement;

public sealed class SuggestionRequest
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Text { get; set; } = string.Empty;
}