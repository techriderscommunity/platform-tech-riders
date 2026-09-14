namespace TechRiders.Domain.Entities;

/// <summary>Catálogo de perfiles principales (Requisitos Arquitectura §3.3): Visitante, Estudiante Tech Activo, Profesor Tech, Orientador, Profesional Tech Junior/Senior, Staff Tajamar.</summary>
public sealed class Profile : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<UserProfileHistory> UserProfileHistories { get; set; } = new List<UserProfileHistory>();
}
