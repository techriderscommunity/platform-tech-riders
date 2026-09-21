using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Estudios actuales de la persona (solo aplica al perfil Estudiante Tech Activo), separado de los intereses tecnológicos.</summary>
public sealed class PersonStudy : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public StudyType StudyType { get; set; }
    public string? Specialty { get; set; }
}
