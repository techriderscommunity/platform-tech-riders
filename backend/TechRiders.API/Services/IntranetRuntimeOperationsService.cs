using TechRiders.Api.Contracts.Requests.Intranet;
using TechRiders.Api.Contracts.Requests.Sessions;

namespace TechRiders.Api.Services;

public interface IIntranetRuntimeOperationsService
{
    void SaveTrace(SaveTraceRequest request);

    IReadOnlyDictionary<string, SessionActionState> GetSessionActions();

    IReadOnlyDictionary<string, SessionActionState> SaveSessionActions(SaveSessionActionsRequest request);

    SessionActionState UpdateSessionWorkflow(Guid sessionId, UpdateSessionWorkflowRequest request);
}

public sealed class IntranetRuntimeOperationsService : IIntranetRuntimeOperationsService
{
    private const string SessionWorkflowKey = "intranet-session-workflow";

    private static readonly string[] AllowedWorkflowStatuses = ["Pendiente", "Confirmada", "Cancelada"];

    private readonly IMvpRuntimeStateStore _mvpRuntimeStateStore;

    public IntranetRuntimeOperationsService(IMvpRuntimeStateStore mvpRuntimeStateStore)
    {
        _mvpRuntimeStateStore = mvpRuntimeStateStore ?? throw new ArgumentNullException(nameof(mvpRuntimeStateStore));
    }

    public void SaveTrace(SaveTraceRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        _mvpRuntimeStateStore.AddTrace(new IntranetTraceEntry
        {
            Kind = request.Kind,
            Route = request.Route,
            Detail = request.Detail,
        });
    }

    public IReadOnlyDictionary<string, SessionActionState> GetSessionActions()
    {
        return _mvpRuntimeStateStore.GetSessionActions(SessionWorkflowKey);
    }

    public IReadOnlyDictionary<string, SessionActionState> SaveSessionActions(SaveSessionActionsRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Actions.Count == 0)
        {
            throw new InvalidOperationException("At least one session action is required.");
        }

        var mappedActions = request.Actions.ToDictionary(
            item => item.Key,
            item => new SessionActionState
            {
                SessionId = item.Key,
                Status = item.Value.Status,
                AmbassadorAssignedId = item.Value.AmbassadorAssignedId,
                UpdatedAt = DateTimeOffset.UtcNow,
            },
            StringComparer.OrdinalIgnoreCase);

        _mvpRuntimeStateStore.UpsertSessionActions(SessionWorkflowKey, mappedActions);
        return mappedActions;
    }

    public SessionActionState UpdateSessionWorkflow(Guid sessionId, UpdateSessionWorkflowRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!string.IsNullOrWhiteSpace(request.Status)
            && !AllowedWorkflowStatuses.Contains(request.Status.Trim(), StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Status must be Pendiente, Confirmada or Cancelada.");
        }

        var workflow = _mvpRuntimeStateStore.GetSessionActions(SessionWorkflowKey)
            .ToDictionary(item => item.Key, item => item.Value, StringComparer.OrdinalIgnoreCase);

        var normalizedSessionId = sessionId.ToString();
        workflow[normalizedSessionId] = new SessionActionState
        {
            SessionId = normalizedSessionId,
            Status = request.Status?.Trim(),
            AmbassadorAssignedId = string.IsNullOrWhiteSpace(request.AmbassadorAssignedId)
                ? null
                : request.AmbassadorAssignedId.Trim(),
            UpdatedAt = DateTimeOffset.UtcNow,
        };

        _mvpRuntimeStateStore.UpsertSessionActions(SessionWorkflowKey, workflow);
        return workflow[normalizedSessionId];
    }
}