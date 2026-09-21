namespace TechRiders.Domain.Entities;

/// <summary>Histórico de perfil principal por persona. Solo una fila por usuario debe tener IsCurrent = true.</summary>
public sealed class UserProfileHistory : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = default!;

    public bool IsCurrent { get; set; } = true;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; }
    public string? Notes { get; set; }
}
