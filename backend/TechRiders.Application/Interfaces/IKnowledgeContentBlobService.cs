namespace TechRiders.Application.Interfaces;

/// <summary>
/// Lee el Markdown de los KnowledgeArticle migrados directamente desde Blob Storage
/// ("al vuelo", sin duplicar el contenido en la base de datos). Sin fallback: si el
/// blob no existe o falla la lectura, propaga la excepci\u00f3n/resultado negativo, nunca
/// devuelve contenido mockeado o de relleno.
/// </summary>
public interface IKnowledgeContentBlobService
{
    /// <summary>
    /// Comprueba si existe el blob referenciado por un art\u00edculo (p.ej. "knowledge/&lt;slug&gt;.md").
    /// </summary>
    Task<bool> ExistsAsync(string blobPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lee el contenido de texto del blob. Lanza si el blob no existe.
    /// </summary>
    Task<string> ReadContentAsync(string blobPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lee el contenido binario (imagenes y otros assets) del blob. Lanza <see cref="FileNotFoundException"/> si no existe.
    /// </summary>
    Task<KnowledgeAssetContent> DownloadBinaryAsync(string blobPath, CancellationToken cancellationToken = default);
}

/// <summary>
/// Contenido binario de un asset de KnowledgeArticle junto con su content type original.
/// </summary>
public sealed record KnowledgeAssetContent(byte[] Content, string ContentType);
