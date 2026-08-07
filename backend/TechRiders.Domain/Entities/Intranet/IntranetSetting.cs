using System.ComponentModel.DataAnnotations;

namespace TechRiders.Domain.Entities.Intranet;

/// <summary>
/// Configuración de la intranet almacenada en base de datos
/// </summary>
public class IntranetSetting : BaseEntity
{
    /// <summary>
    /// Clave única del setting
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Módulo o sección a la que pertenece
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// Valor del setting
    /// </summary>
    [Required]
    [StringLength(4000)]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Estado del setting (activo/inactivo)
    /// </summary>
    [StringLength(50)]
    public string Status { get; set; } = "activo";

    /// <summary>
    /// Fecha UTC de última actualización
    /// </summary>
    public DateTime UpdatedUtc { get; set; }

    /// <summary>
    /// Usuario que actualizó el setting
    /// </summary>
    [StringLength(255)]
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Token para concurrencia optimista
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    protected IntranetSetting()
    {
    }

    /// <summary>
    /// Factory method para crear un nuevo setting
    /// </summary>
    public static IntranetSetting Create(
        string key,
        string module,
        string value,
        string status,
        string? updatedBy)
    {
        return new IntranetSetting
        {
            Key = key,
            Module = module,
            Value = value,
            Status = status,
            UpdatedBy = updatedBy,
            UpdatedUtc = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Actualiza el setting
    /// </summary>
    public void Update(string module, string value, string status, string? updatedBy)
    {
        Module = module;
        Value = value;
        Status = status;
        UpdatedBy = updatedBy;
        UpdatedUtc = DateTime.UtcNow;
    }
}
