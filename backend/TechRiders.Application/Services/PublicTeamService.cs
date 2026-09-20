using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

public sealed class PublicTeamService : IPublicTeamService
{
    // Mapeo zona -> Capability real del catalogo seedeado (IdentityCatalogSeedService: Staff, Community Leader, Ambassador, Center, Community Partner).
    // TODO(gap): "member" no tiene Capability equivalente (Member es automatico, sin solicitud/capability propia);
    // definir con producto de donde debe salir ese grupo.
    private static readonly IReadOnlyDictionary<string, string> ZoneToCapabilityName = new Dictionary<string, string>
    {
        ["staff"] = "Staff",
        ["community-leaders"] = "Community Leader",
        ["ambassador"] = "Ambassador",
    };

    private readonly IUnitOfWork _unitOfWork;

    public PublicTeamService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<IReadOnlyList<PublicTeamZoneResponse>> GetTeamZonesAsync(CancellationToken cancellationToken = default)
    {
        var zones = new List<PublicTeamZoneResponse>();

        foreach (var (zoneKey, capabilityName) in ZoneToCapabilityName)
        {
            var users = await _unitOfWork.Users.GetActiveByCapabilityNameAsync(capabilityName, cancellationToken);
            zones.Add(new PublicTeamZoneResponse
            {
                Key = zoneKey,
                Members = users.Select(ToMember).ToArray(),
            });
        }

        return zones;
    }

    private static PublicTeamMemberResponse ToMember(User user) => new()
    {
        Name = user.Name,
        LastName = user.LastName,
        LinkedIn = user.LinkedIn,
        Instagram = user.Instagram,
        X = user.X,
        YouTube = user.YouTube,
        Github = user.Github,
    };
}
