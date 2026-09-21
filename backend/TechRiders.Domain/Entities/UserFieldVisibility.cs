using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Visibilidad configurada por la persona para un campo concreto de su perfil (Requisitos Arquitectura §10). Por defecto Private hasta configuración explícita.</summary>
public sealed class UserFieldVisibility : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public required string FieldKey { get; set; }
    public FieldVisibility Visibility { get; set; } = FieldVisibility.Private;
}
