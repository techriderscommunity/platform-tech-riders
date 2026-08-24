using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests;

public class UpdateOfertaRequest
{
    [Required]
    public Guid Id { get; set; }

    [StringLength(200)]
    public string? Titulo { get; set; }

    [StringLength(200)]
    public string? Empresa { get; set; }

    public string? Descripcion { get; set; }

    [Range(0, 10000000)]
    public decimal? Salario { get; set; }

    [StringLength(200)]
    public string? Ubicacion { get; set; }

    [Range(0, 2)]
    public int? Modalidad { get; set; }

    public string? Requisitos { get; set; }

    [Range(0, 2)]
    public int? Estado { get; set; }
}
