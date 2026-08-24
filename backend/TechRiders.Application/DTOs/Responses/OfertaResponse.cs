namespace TechRiders.Application.DTOs.Responses;

public class OfertaResponse
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Empresa { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public string Ubicacion { get; set; } = string.Empty;
    public int Modalidad { get; set; }
    public string Requisitos { get; set; } = string.Empty;
    public int Estado { get; set; }
    public DateTime FechaPublicacion { get; set; }
}
