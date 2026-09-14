using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Organización (centro, empresa, comunidad, etc.). No es una persona ni un rol de usuario (Requisitos Arquitectura §6).</summary>
public sealed class Organization : BaseEntity
{
    public OrganizationType OrganizationType { get; set; }
    public required string Name { get; set; }
    public string? TaxId { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public string? Province { get; set; }
    public string? Origin { get; set; }

    public ICollection<PersonOrganization> PersonRelations { get; set; } = new List<PersonOrganization>();
    public OrganizationGpfLink? GpfLink { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<FPTour> FPTours { get; set; } = new List<FPTour>();
}
