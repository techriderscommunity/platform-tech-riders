using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests;

public class CreateOfertaRequest
{
    [Required]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Empresa { get; set; } = string.Empty;

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0, 10000000)]
    public decimal Salario { get; set; }

    [StringLength(200)]
    public string Ubicacion { get; set; } = string.Empty;

    [Range(0, 2)]
    public int Modalidad { get; set; }

    public string Requisitos { get; set; } = string.Empty;
}
