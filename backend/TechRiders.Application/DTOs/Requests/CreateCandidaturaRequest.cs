using System.ComponentModel.DataAnnotations;

namespace TechRiders.Application.DTOs.Requests;

public class CreateCandidaturaRequest
{
    [Required]
    public Guid OfertaId { get; set; }

    [Required]
    public string JuniorId { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string NombreJunior { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string EmailJunior { get; set; } = string.Empty;
}
