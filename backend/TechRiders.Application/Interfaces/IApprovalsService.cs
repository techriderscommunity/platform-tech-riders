namespace TechRiders.Application.Interfaces;

public sealed record ApprovalItem(Guid Id, string Type, string Title, string? RequestedBy, DateTime RequestedAt, string Module);

/// <summary>Bandeja centralizada de elementos pendientes de revision (capacidades, relaciones org, comuneras, centros publicos).</summary>
public interface IApprovalsService
{
    Task<List<ApprovalItem>> GetPendingAsync(CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetPendingCountsByTypeAsync(CancellationToken cancellationToken = default);
}
