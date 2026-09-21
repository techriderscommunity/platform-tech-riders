namespace TechRiders.Domain.Entities;

/// <summary>Finalidad de tratamiento (Requisitos Arquitectura §8.2): participación, comunicaciones operativas, comunidad, sesiones, perfil público, imagen/grabaciones, empleo, marketplace.</summary>
public sealed class ConsentPurpose : BaseEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
