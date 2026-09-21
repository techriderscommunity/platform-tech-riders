using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Ámbito(s) de orientación de la persona con perfil Orientador (multi-selección, Requisitos Arquitectura §3.4).</summary>
public sealed class UserOrientationScope : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public OrientationScope Scope { get; set; }
    public string? OtherDetail { get; set; }
}
