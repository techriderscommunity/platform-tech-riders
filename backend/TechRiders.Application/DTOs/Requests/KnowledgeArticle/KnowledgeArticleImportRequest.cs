namespace TechRiders.Application.DTOs.Requests.KnowledgeArticle;

/// <summary>
/// Un art\u00edculo del manifest.json generado por scripts/intake/wp-import.
/// </summary>
public sealed class KnowledgeArticleImportItemRequest
{
    public required string Title { get; set; }
    public required string Slug { get; set; }

    /// <summary>
    /// Ruta del blob en el container "knowledge" (p.ej. "knowledge/&lt;slug&gt;.md"),
    /// subido a mano por el operador antes de llamar a este import.
    /// </summary>
    public required string ContentBlobPath { get; set; }

    public DateTimeOffset? PublishedAt { get; set; }
    public List<string> CategoryNames { get; set; } = new();
    public List<string> SkillNames { get; set; } = new();
}

public sealed class KnowledgeArticleImportRequest
{
    public required List<KnowledgeArticleImportItemRequest> Items { get; set; }
}
