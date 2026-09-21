using TechRiders.Application.DTOs.Requests.KnowledgeArticle;
using TechRiders.Application.DTOs.Responses.KnowledgeArticle;

namespace TechRiders.Application.Interfaces;

/// <summary>
/// Servicio de negocio para KnowledgeArticle (tutoriales migrados desde WordPress y
/// los que se creen desde ahora en la plataforma).
/// </summary>
public interface IKnowledgeArticleService
{
    Task<PagedKnowledgeArticleResponse> GetPagedAsync(
        int page,
        int pageSize,
        Guid? categoryId,
        Guid? skillId,
        string? search,
        string? categoryName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Devuelve null si el slug no existe. Lanza <see cref="Exceptions.KnowledgeContentUnavailableException"/>
    /// si el art\u00edculo existe pero su blob de contenido no se puede leer.
    /// </summary>
    Task<KnowledgeArticleResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<KnowledgeArticleResponse?> UpdateAsync(Guid id, UpdateKnowledgeArticleRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Upsert idempotente por Slug. Rechaza (sin insertar) los art\u00edculos cuyo blob de
    /// contenido no exista todav\u00eda; el operador debe subirlo y reintentar.
    /// </summary>
    Task<KnowledgeArticleImportResultResponse> ImportAsync(KnowledgeArticleImportRequest request, CancellationToken cancellationToken = default);
}
