using System.ComponentModel.DataAnnotations;

namespace TechRiders.Domain.Entities.Empleo;

/// <summary>
/// Representa una oferta de empleo de TechRiders
/// </summary>
public class Oferta : BaseEntity
{
    /// <summary>
    /// Título de la oferta
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Titulo { get; set; } = string.Empty;

    /// <summary>
    /// Empresa que publica la oferta
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Empresa { get; set; } = string.Empty;

    /// <summary>
    /// Salario ofrecido
    /// </summary>
    [StringLength(255)]
    public string Salario { get; set; } = string.Empty;

    /// <summary>
    /// Ubicación del puesto
    /// </summary>
    [StringLength(255)]
    public string Ubicacion { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada de la oferta
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Requisitos para el puesto
    /// </summary>
    public string Requisitos { get; set; } = string.Empty;

    /// <summary>
    /// Modalidad de trabajo
    /// </summary>
    public Modalidad Modalidad { get; set; }

    /// <summary>
    /// Estado actual de la oferta
    /// </summary>
    public OfertaEstado Estado { get; set; }

    /// <summary>
    /// Fecha de publicación de la oferta
    /// </summary>
    public DateTime FechaPublicacion { get; set; }

    /// <summary>
    /// Token para concurrencia optimista
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    protected Oferta()
    {
    }

    /// <summary>
    /// Factory method para crear una nueva oferta
    /// </summary>
    public static Oferta Create(string titulo, string empresa, string descripcion, decimal salario, string ubicacion, Modalidad modalidad, string requisitos)
    {
        return new Oferta
        {
            Titulo = titulo,
            Empresa = empresa,
            Descripcion = descripcion,
            Salario = salario.ToString("F2"),
            Ubicacion = ubicacion,
            Modalidad = modalidad,
            Requisitos = requisitos,
            Estado = OfertaEstado.Borrador,
            FechaPublicacion = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Publica la oferta
    /// </summary>
    public void Publicar()
    {
        Estado = OfertaEstado.Activa;
    }

    /// <summary>
    /// Cierra la oferta
    /// </summary>
    public void Cerrar()
    {
        Estado = OfertaEstado.Cerrada;
    }
}
