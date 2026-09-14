namespace TechRiders.Domain.Entities;

/// <summary>Catálogo de bases jurídicas. La arquitectura permite registrarlas; el DPO decide cuál aplica a cada finalidad.</summary>
public sealed class LegalBasis : BaseEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
