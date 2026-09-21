namespace TechRiders.Application.Exceptions;

/// <summary>
/// El KnowledgeArticle existe en BD pero su Markdown no se pudo leer de Blob Storage
/// (blob inexistente o error de lectura). Sin fallback: nunca se sirve contenido
/// mockeado; el controller debe traducir esto a un error real (502/404 seg\u00fan el caso).
/// </summary>
public sealed class KnowledgeContentUnavailableException : Exception
{
    public KnowledgeContentUnavailableException(string blobPath, Exception? inner = null)
        : base($"No se pudo leer el contenido del blob '{blobPath}'.", inner)
    {
        BlobPath = blobPath;
    }

    public string BlobPath { get; }
}
