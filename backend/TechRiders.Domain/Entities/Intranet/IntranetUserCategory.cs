using System.ComponentModel.DataAnnotations;

namespace TechRiders.Domain.Entities.Intranet;

/// <summary>
/// Categoría de usuario en la intranet
/// </summary>
public class IntranetUserCategory : BaseEntity
{
    /// <summary>
    /// ID del usuario en Azure AD o SQL
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Nombre de la categoría/rol
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la categoría
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Indica si la categoría está activa
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    /// Token para concurrencia optimista
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    protected IntranetUserCategory()
    {
    }

    /// <summary>
    /// Factory method para crear una nueva categoría de usuario
    /// </summary>
    public static IntranetUserCategory Create(Guid userId, string category, string? description)
    {
        return new IntranetUserCategory
        {
            UserId = userId,
            Category = category,
            Description = description,
            Active = true
        };
    }
}
