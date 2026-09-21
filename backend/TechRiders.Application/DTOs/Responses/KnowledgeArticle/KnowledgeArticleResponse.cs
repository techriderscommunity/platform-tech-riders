namespace TechRiders.Application.DTOs.Responses.KnowledgeArticle;

/// <summary>
/// Metadata de listado (sin contenido; no toca Blob Storage).
/// </summary>
public sealed class KnowledgeArticleSummaryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTimeOffset? PublishedAt { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? StatusName { get; set; }
    public List<string> Categories { get; set; } = new();
    public List<string> Skills { get; set; } = new();
}

public sealed class PagedKnowledgeArticleResponse
{
    public IReadOnlyList<KnowledgeArticleSummaryResponse> Items { get; set; } = Array.Empty<KnowledgeArticleSummaryResponse>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}

/// <summary>
/// Detalle de un art\u00edculo: metadata desde BD + Markdown resuelto al vuelo desde Blob Storage.
/// </summary>
public sealed class KnowledgeArticleResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ContentMd { get; set; } = string.Empty;
    public DateTimeOffset? PublishedAt { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? StatusName { get; set; }
    public List<string> Categories { get; set; } = new();
    public List<string> Skills { get; set; } = new();
}
