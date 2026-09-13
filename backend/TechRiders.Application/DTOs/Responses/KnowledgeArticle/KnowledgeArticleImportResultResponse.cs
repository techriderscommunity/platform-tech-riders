namespace TechRiders.Application.DTOs.Responses.KnowledgeArticle;

public sealed class KnowledgeArticleImportItemResult
{
    public required string Slug { get; set; }
    public bool Imported { get; set; }
    public string? Error { get; set; }
}

/// <summary>
/// Resultado del import batch. Sin fallback: cada item rechazado queda expl\u00edcito en
/// <see cref="Rejected"/> con el motivo, nunca se inserta contenido vac\u00edo/placeholder.
/// </summary>
public sealed class KnowledgeArticleImportResultResponse
{
    public int Requested { get; set; }
    public int Imported { get; set; }
    public int Rejected { get; set; }
    public List<KnowledgeArticleImportItemResult> Items { get; set; } = new();
}
