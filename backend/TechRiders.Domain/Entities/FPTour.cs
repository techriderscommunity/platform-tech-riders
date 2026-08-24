using System.ComponentModel.DataAnnotations.Schema;

namespace TechRiders.Domain.Entities;

public sealed class FPTour : BaseEntity
{
    public Guid CenterId { get; set; }
    public Center Center { get; set; } = default!;

    public Guid? AmbassadorUserId { get; set; }
    public User? Ambassador { get; set; }

    // Compatibilidad con nombres legacy usados por repositorios y servicios.
    // No se mapea a BD porque EF Core considera que el nombre de la FK ya existe y genera
    // la propiedad shadow 'AmbassadorId1' cuando se expone como alias del mismo campo.
    [NotMapped]
    public Guid? AmbassadorId
    {
        get => AmbassadorUserId;
        set => AmbassadorUserId = value;
    }

    public DateTimeOffset? PlannedDate { get; set; }
    public bool HasScheduledDate { get => PlannedDate.HasValue; set { if (!value) PlannedDate = null; } }
    public string? Notes { get; set; }

    public Guid? StatusId { get; set; }
    public Status? Status { get; set; }

    public ICollection<FPTourTask> Tasks { get; set; } = new List<FPTourTask>();
}
