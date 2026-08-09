using System.Collections.Concurrent;

namespace TechRiders.Api.Services;

/// <summary>
/// Repositorio de runtime para intranet. Implementación actual: in-memory.
/// Se sustituirá por persistencia real al conectar BBDD database-first.
/// </summary>
public interface IMvpRuntimeRepository
{
    MemberProfileState GetOrCreateMemberProfile(string userKey, string? fallbackEmail);

    void UpsertMemberProfile(string userKey, MemberProfileState profile);

    AmbassadorPortalState GetOrCreateAmbassadorPortal(string userKey, string? fallbackEmail);

    void UpsertAmbassadorPortal(string userKey, AmbassadorPortalState profile);

    IReadOnlyList<string> GetUserCategories(string userKey);

    void UpsertUserCategories(string userKey, IReadOnlyList<string> categories);

    void AddTrace(IntranetTraceEntry traceEntry);

    IReadOnlyDictionary<string, SessionActionState> GetSessionActions(string userKey);

    void UpsertSessionActions(string userKey, IReadOnlyDictionary<string, SessionActionState> actions);
}

/// <summary>
/// Implementación in-memory para desarrollo local hasta tener persistencia definitiva.
/// </summary>
public sealed class InMemoryMvpRuntimeRepository : IMvpRuntimeRepository
{
    private static readonly string[] DefaultCategories = ["FP Tour", "Eventos", "Mentorías", "Podcast", "Comunidad"];

    private static readonly Dictionary<string, SessionActionState> DefaultSessionActions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["demo-session-1"] = new SessionActionState
        {
            SessionId = "demo-session-1",
            Status = "Pendiente",
            AmbassadorAssignedId = null,
            UpdatedAt = DateTimeOffset.UtcNow,
        },
        ["demo-session-2"] = new SessionActionState
        {
            SessionId = "demo-session-2",
            Status = "Confirmada",
            AmbassadorAssignedId = "amb-001",
            UpdatedAt = DateTimeOffset.UtcNow,
        },
    };

    private readonly ConcurrentDictionary<string, MemberProfileState> memberProfiles = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, AmbassadorPortalState> ambassadorProfiles = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, string[]> userCategories = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, Dictionary<string, SessionActionState>> sessionActions = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentQueue<IntranetTraceEntry> traces = new();

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

    public void UpsertMemberProfile(string userKey, MemberProfileState profile)
    {
        var normalizedKey = NormalizeKey(userKey, profile.Email);
        this.memberProfiles[normalizedKey] = profile with { Email = profile.Email.Trim() };
    }

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

    public void UpsertAmbassadorPortal(string userKey, AmbassadorPortalState profile)
    {
        var normalizedKey = NormalizeKey(userKey, profile.Email);
        this.ambassadorProfiles[normalizedKey] = profile with { Email = profile.Email.Trim() };
    }

    public IReadOnlyList<string> GetUserCategories(string userKey)
    {
        var normalizedKey = NormalizeKey(userKey, null);
        return this.userCategories.TryGetValue(normalizedKey, out var categories)
            ? categories
            : DefaultCategories;
    }

    public void UpsertUserCategories(string userKey, IReadOnlyList<string> categories)
    {
        var normalizedKey = NormalizeKey(userKey, null);
        this.userCategories[normalizedKey] = categories
            .Where(category => !string.IsNullOrWhiteSpace(category))
            .Select(category => category.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public void AddTrace(IntranetTraceEntry traceEntry)
    {
        this.traces.Enqueue(traceEntry with { Timestamp = DateTimeOffset.UtcNow });
    }

    public IReadOnlyDictionary<string, SessionActionState> GetSessionActions(string userKey)
    {
        var normalizedKey = NormalizeKey(userKey, null);
        return this.sessionActions.TryGetValue(normalizedKey, out var actions)
            ? new Dictionary<string, SessionActionState>(actions, StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, SessionActionState>(DefaultSessionActions, StringComparer.OrdinalIgnoreCase);
    }

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

public sealed record AmbassadorPortalState
{
    public string Email { get; init; } = string.Empty;

    public string Bio { get; init; } = string.Empty;

    public string Specialties { get; init; } = string.Empty;

    public string Availability { get; init; } = string.Empty;
}

public sealed record IntranetTraceEntry
{
    public string Kind { get; init; } = string.Empty;

    public string Route { get; init; } = string.Empty;

    public string Detail { get; init; } = string.Empty;

    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}

public sealed record SessionActionState
{
    public string SessionId { get; init; } = string.Empty;

    public string? Status { get; init; }

    public string? AmbassadorAssignedId { get; init; }

    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}