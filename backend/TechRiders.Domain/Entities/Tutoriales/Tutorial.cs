using System.ComponentModel.DataAnnotations;

namespace TechRiders.Domain.Entities.Tutoriales;

/// <summary>
/// Representa un tutorial en TechRiders
/// </summary>
public class Tutorial : BaseEntity
{
    /// <summary>
    /// Slug único del tutorial (URL-friendly)
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Título del tutorial
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Titulo { get; set; } = string.Empty;

    /// <summary>
    /// Extracto o descripción breve
    /// </summary>
    [StringLength(1000)]
    public string Extracto { get; set; } = string.Empty;

    /// <summary>
    /// Autor del tutorial
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Autor { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de publicación
    /// </summary>
    public DateTime FechaPublicacion { get; set; }

    /// <summary>
    /// Categorías (almacenadas como JSON en la base de datos)
    /// </summary>
    [StringLength(2000)]
    public string CategoriasJson { get; set; } = "[]";

    /// <summary>
    /// URL del tutorial o contenido
    /// </summary>
    [StringLength(500)]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Token para concurrencia optimista
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    protected Tutorial()
    {
    }

    /// <summary>
    /// Factory method para crear un nuevo tutorial
    /// </summary>
    public static Tutorial Create(
        string slug,
        string titulo,
        string extracto,
        string autor,
        string categoriasJson,
        string url)
    {
        return new Tutorial
        {
            Slug = slug,
            Titulo = titulo,
            Extracto = extracto,
            Autor = autor,
            FechaPublicacion = DateTime.UtcNow,
            CategoriasJson = categoriasJson ?? "[]",
            Url = url
        };
    }

    /// <summary>
    /// Obtiene las categorías como lista
    /// </summary>
    public IReadOnlyList<string> GetCategorias()
    {
        try
        {
            var items = System.Text.Json.JsonSerializer.Deserialize<List<string>>(CategoriasJson);
            return items?.AsReadOnly() ?? new List<string>().AsReadOnly();
        }
        catch
        {
            return new List<string>().AsReadOnly();
        }
    }
}
