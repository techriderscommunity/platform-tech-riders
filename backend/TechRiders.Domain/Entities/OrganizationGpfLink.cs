using TechRiders.Domain.Enums;

namespace TechRiders.Domain.Entities;

/// <summary>
/// Vínculo opcional de una <see cref="Organization"/> con su equivalente en GPF.
/// El identificador externo se encapsula aquí porque Infortécnica aún no ha confirmado su nombre
/// oficial (referido provisionalmente como "IdOrganizacionGPF" en el documento de decisiones);
/// ningún otro punto del dominio debe conocer ese nombre provisional.
/// </summary>
public sealed class OrganizationGpfLink : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    /// <summary>Identificador de organización en GPF. Nullable: no todas las organizaciones lo tendrán.</summary>
    public string? GpfReference { get; set; }

    public GpfLinkStatus Status { get; set; } = GpfLinkStatus.Activo;
    public DateTime LinkedAt { get; set; } = DateTime.UtcNow;
    public string? LinkMethod { get; set; }
    public Guid? ValidatedByUserId { get; set; }
}
