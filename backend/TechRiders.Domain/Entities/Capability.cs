namespace TechRiders.Domain.Entities;

/// <summary>Catálogo de capacidades acumulables (Requisitos Arquitectura §3.5): Tech Rider, Orientador de centro, Representante de empresa, Mentor, Creador de contenido, Colaborador/Embajador, Staff Operativo.</summary>
public sealed class Capability : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool RequiresValidation { get; set; } = true;

    public ICollection<UserCapability> UserCapabilities { get; set; } = new List<UserCapability>();
}
