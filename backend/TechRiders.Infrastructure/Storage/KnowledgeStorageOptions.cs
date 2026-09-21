namespace TechRiders.Infrastructure.Storage;

/// <summary>
/// Opciones de conexión al storage account que archiva el contenido histórico de KnowledgeArticle.
/// </summary>
public sealed class KnowledgeStorageOptions
{
    public const string SectionName = "Storage";

    /// <summary>
    /// Connection string opcional del storage account. Se usa preferentemente cuando la app se ejecuta
    /// localmente o en entornos sin identity gestionada. En Azure con managed identity, puede dejarse nula.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// URL base del storage account, p.ej. https://storagetetxito.blob.core.windows.net
    /// </summary>
    public required string AccountUrl { get; set; }

    /// <summary>
    /// Container donde se suben a mano los .md y las imágenes migradas ("knowledge").
    /// </summary>
    public required string KnowledgeContainer { get; set; }

    /// <summary>Container privado para fotos de usuarios y logos de Comuneras.</summary>
    public string ProfileContainer { get; set; } = "profiles";
}
