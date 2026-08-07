using System.Collections.Concurrent;

namespace TechRiders.Api.Services;

/// <summary>
/// Contract for the in-memory runtime state used by the local MVP flows.
/// </summary>
public interface IMvpRuntimeStateStore
{
    /// <summary>
    /// Returns an existing member profile or creates a default profile for the supplied key.
    /// </summary>
    MemberProfileState GetOrCreateMemberProfile(string userKey, string? fallbackEmail);

    /// <summary>
    /// Persists the current member profile state for the supplied key.
    /// </summary>
    void UpsertMemberProfile(string userKey, MemberProfileState profile);

    /// <summary>
    /// Returns an existing ambassador portal state or creates a default one for the supplied key.
    /// </summary>
    AmbassadorPortalState GetOrCreateAmbassadorPortal(string userKey, string? fallbackEmail);

    /// <summary>
    /// Persists the current ambassador portal state for the supplied key.
    /// </summary>
    void UpsertAmbassadorPortal(string userKey, AmbassadorPortalState profile);

    /// <summary>
    /// Returns the user categories stored for the supplied key.
    /// </summary>
    IReadOnlyList<string> GetUserCategories(string userKey);

    /// <summary>
    /// Replaces the user categories stored for the supplied key.
    /// </summary>
    void UpsertUserCategories(string userKey, IReadOnlyList<string> categories);

    /// <summary>
    /// Appends a local trace entry for intranet usage insights.
    /// </summary>
    void AddTrace(IntranetTraceEntry traceEntry);

    /// <summary>
    /// Returns the current session action overrides for the supplied key.
    /// </summary>
    IReadOnlyDictionary<string, SessionActionState> GetSessionActions(string userKey);

    /// <summary>
    /// Replaces the session action overrides for the supplied key.
    /// </summary>
    void UpsertSessionActions(string userKey, IReadOnlyDictionary<string, SessionActionState> actions);
}

/// <summary>
/// In-memory store used to keep local MVP state while Azure SQL and SharePoint integrations are not available.
/// </summary>
public sealed class InMemoryMvpRuntimeStateStore : IMvpRuntimeStateStore
{
    private readonly ConcurrentDictionary<string, MemberProfileState> memberProfiles = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, AmbassadorPortalState> ambassadorProfiles = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, string[]> userCategories = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, Dictionary<string, SessionActionState>> sessionActions = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentQueue<IntranetTraceEntry> traces = new();

    /// <inheritdoc />
    public MemberProfileState GetOrCreateMemberProfile(string userKey, string? fallbackEmail)
    {
        var normalizedKey = NormalizeKey(userKey, fallbackEmail);
        return this.memberProfiles.GetOrAdd(normalizedKey, _ => new MemberProfileState
        {
            Name = string.Empty,
            Email = fallbackEmail ?? normalizedKey,
            Bio = string.Empty,
            Interests = "FP Tour, eventos, comunidad y aprendizaje práctico",
            Audience = "junior",
            CommunityRole = "member",
            Organization = string.Empty,
        });
    }

    /// <inheritdoc />
    public void UpsertMemberProfile(string userKey, MemberProfileState profile)
    {
        var normalizedKey = NormalizeKey(userKey, profile.Email);
        this.memberProfiles[normalizedKey] = profile with { Email = profile.Email.Trim() };
    }

    /// <inheritdoc />
    public AmbassadorPortalState GetOrCreateAmbassadorPortal(string userKey, string? fallbackEmail)
    {
        var normalizedKey = NormalizeKey(userKey, fallbackEmail);
        return this.ambassadorProfiles.GetOrAdd(normalizedKey, _ => new AmbassadorPortalState
        {
            Email = fallbackEmail ?? normalizedKey,
            Bio = "Me interesa aportar sesiones prácticas, orientación y comunidad alrededor de tecnología real.",
            Specialties = "Cloud, desarrollo web, mentoring, empleabilidad",
            Availability = "Martes y jueves por la tarde; viernes por la mañana con aviso previo.",
        });
    }

    /// <inheritdoc />
    public void UpsertAmbassadorPortal(string userKey, AmbassadorPortalState profile)
    {
        var normalizedKey = NormalizeKey(userKey, profile.Email);
        this.ambassadorProfiles[normalizedKey] = profile with { Email = profile.Email.Trim() };
    }

    /// <inheritdoc />
    public IReadOnlyList<string> GetUserCategories(string userKey)
    {
        var normalizedKey = NormalizeKey(userKey, null);
        return this.userCategories.TryGetValue(normalizedKey, out var categories)
            ? categories
            : [];
    }

    /// <inheritdoc />
    public void UpsertUserCategories(string userKey, IReadOnlyList<string> categories)
    {
        var normalizedKey = NormalizeKey(userKey, null);
        this.userCategories[normalizedKey] = categories
            .Where(category => !string.IsNullOrWhiteSpace(category))
            .Select(category => category.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    /// <inheritdoc />
    public void AddTrace(IntranetTraceEntry traceEntry)
    {
        this.traces.Enqueue(traceEntry with { Timestamp = DateTimeOffset.UtcNow });
    }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, SessionActionState> GetSessionActions(string userKey)
    {
        var normalizedKey = NormalizeKey(userKey, null);
        return this.sessionActions.TryGetValue(normalizedKey, out var actions)
            ? new Dictionary<string, SessionActionState>(actions, StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, SessionActionState>(StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public void UpsertSessionActions(string userKey, IReadOnlyDictionary<string, SessionActionState> actions)
    {
        var normalizedKey = NormalizeKey(userKey, null);
        this.sessionActions[normalizedKey] = actions.ToDictionary(
            item => item.Key,
            item => item.Value,
            StringComparer.OrdinalIgnoreCase);
    }

    private static string NormalizeKey(string? userKey, string? fallbackEmail)
    {
        var candidate = string.IsNullOrWhiteSpace(userKey) ? fallbackEmail : userKey;
        return string.IsNullOrWhiteSpace(candidate)
            ? "local-user@techriders.local"
            : candidate.Trim().ToLowerInvariant();
    }
}

/// <summary>
/// Mutable member profile state used by the local MVP runtime.
/// </summary>
public sealed record MemberProfileState
{
    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Bio { get; init; } = string.Empty;

    public string Interests { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string CommunityRole { get; init; } = string.Empty;

    public string Organization { get; init; } = string.Empty;
}

/// <summary>
/// Mutable ambassador portal state used by the local MVP runtime.
/// </summary>
public sealed record AmbassadorPortalState
{
    public string Email { get; init; } = string.Empty;

    public string Bio { get; init; } = string.Empty;

    public string Specialties { get; init; } = string.Empty;

    public string Availability { get; init; } = string.Empty;
}

/// <summary>
/// Trace payload used by the intranet MVP.
/// </summary>
public sealed record IntranetTraceEntry
{
    public string Kind { get; init; } = string.Empty;

    public string Route { get; init; } = string.Empty;

    public string Detail { get; init; } = string.Empty;

    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Session override stored locally for the MVP session workflow.
/// </summary>
public sealed record SessionActionState
{
    public string SessionId { get; init; } = string.Empty;

    public string? Status { get; init; }

    public string? AmbassadorAssignedId { get; init; }

    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
