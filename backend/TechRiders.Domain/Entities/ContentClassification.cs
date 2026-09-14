using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>Clasifica un contenido/actividad (Event, Session o KnowledgeArticle) con la misma taxonomía que las preferencias.</summary>
public sealed class ContentClassification : BaseEntity
{
    public ContentKind ContentKind { get; set; }
    public Guid ContentId { get; set; }

    public Guid DimensionValueId { get; set; }
    public PreferenceDimensionValue DimensionValue { get; set; } = default!;
}
