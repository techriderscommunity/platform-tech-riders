namespace TechRiders.Domain.Entities.Tutoriales;

/// <summary>
/// Resultado paginado de tutoriales
/// </summary>
public sealed record TutorialesPageResult(
    IReadOnlyList<Tutorial> Items,
    int TotalCount);
