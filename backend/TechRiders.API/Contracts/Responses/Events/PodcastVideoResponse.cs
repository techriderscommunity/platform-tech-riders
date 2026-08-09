using System.Text.Json.Serialization;

namespace TechRiders.Api.Contracts.Responses.Events;

public sealed class PodcastVideoResponse
{
    [JsonPropertyName("videoId")]
    public string VideoId { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("embedUrl")]
    public string EmbedUrl { get; set; } = string.Empty;

    [JsonPropertyName("publishedAt")]
    public DateTime? PublishedAt { get; set; }

    [JsonPropertyName("thumbnailUrl")]
    public string? ThumbnailUrl { get; set; }
}