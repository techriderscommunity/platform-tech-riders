using TechRiders.Application.DTOs.Responses.Public;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Interfaces;

namespace TechRiders.Application.Services;

public sealed class PublicTeamService : IPublicTeamService
{
    private const int RandomSampleSize = 8;

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
            if (zoneKey == "ambassador")
            {
                users = users.OrderBy(_ => Guid.NewGuid()).Take(RandomSampleSize).ToArray();
            }
            else
            {
                users = users.OrderBy(user => user.LastName).ThenBy(user => user.Name).ToArray();
            }

            zones.Add(new PublicTeamZoneResponse
            {
                Key = zoneKey,
                Members = users.Select(ToMember).ToArray(),
            });
        }

        var members = await _unitOfWork.Users.GetActiveMembersAsync(cancellationToken);
        zones.Add(new PublicTeamZoneResponse
        {
            Key = "member",
            Members = members
                .OrderBy(_ => Guid.NewGuid())
                .Take(RandomSampleSize)
                .Select(ToMember)
                .ToArray(),
        });

        return zones;
    }

    private static PublicTeamMemberResponse ToMember(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        LastName = user.LastName,
        About = user.About,
        LinkedIn = user.LinkedIn,
        Instagram = user.Instagram,
        X = user.X,
        YouTube = user.YouTube,
        Github = user.Github,
    };
}
