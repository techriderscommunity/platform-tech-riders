using System.ComponentModel.DataAnnotations;

namespace TechRiders.Domain.Entities;

/// <summary>
/// Representa una categoría maestra (tabla MT_Category)
/// Puede ser categoría padre o subcategoría (relación jerárquica)
/// </summary>
public class MT_Category
{
    /// <summary>
    /// Identificador único de la categoría
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Nombre de la categoría
    /// </summary>
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la categoría padre (null si es categoría principal)
    /// </summary>
    public int? FatherId { get; set; }

    /// <summary>
    /// Indica si la categoría está activa
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    /// Categoría padre (navegación)
    /// </summary>
    public virtual MT_Category? Main { get; set; }

    /// <summary>
    /// Subcategorías hijas (navegación)
    /// </summary>
    public virtual ICollection<MT_Category> Secondary { get; set; } = new List<MT_Category>();

    /// <summary>
    /// Ambassadors que usan esta categoría
    /// </summary>
    public virtual ICollection<Ambassador> Ambassadors { get; set; } = new List<Ambassador>();
}
