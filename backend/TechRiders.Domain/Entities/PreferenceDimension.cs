namespace TechRiders.Domain.Entities;

/// <summary>Dimensión de la taxonomía multidimensional (Requisitos Arquitectura §5): Tecnología, Tipo de contenido, Modalidad, Ubicación, Nivel, Objetivo, Canal, Frecuencia.</summary>
public sealed class PreferenceDimension : BaseEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<PreferenceDimensionValue> Values { get; set; } = new List<PreferenceDimensionValue>();
}
