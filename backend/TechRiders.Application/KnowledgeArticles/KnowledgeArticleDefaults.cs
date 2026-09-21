namespace TechRiders.Application.KnowledgeArticles;

/// <summary>
/// Constantes compartidas entre el seed (arranque de la API) y el servicio de import,
/// para identificar el usuario/estado usados por el contenido migrado de WordPress.
/// </summary>
public static class KnowledgeArticleDefaults
{
    public const string MigrationAuthorEmail = "wp-import@techriders.local";
    public const string StatusScope = "KnowledgeArticle";
    public const string PublishedStatusName = "Published";
    public const string FallbackCategoryName = "Sin categor\u00eda";
}
