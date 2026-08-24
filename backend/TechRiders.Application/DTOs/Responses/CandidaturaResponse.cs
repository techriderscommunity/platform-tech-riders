namespace TechRiders.Application.DTOs.Responses;

public class CandidaturaResponse
{
    public Guid Id { get; set; }
    public Guid OfertaId { get; set; }
    public string JuniorId { get; set; } = string.Empty;
    public string NombreJunior { get; set; } = string.Empty;
    public string EmailJunior { get; set; } = string.Empty;
    public int Estado { get; set; }
    public DateTime FechaSolicitud { get; set; }
}
