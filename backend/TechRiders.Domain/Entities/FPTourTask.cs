using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

public sealed class FPTourTask : BaseEntity
{
    public Guid FPTourId { get; set; }
    public FPTour FPTour { get; set; } = default!;

    public FPTourTaskType TaskType { get; set; } = FPTourTaskType.Custom;
    public required string Name { get; set; }
    public bool Completed { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public string? Notes { get; set; }
}
