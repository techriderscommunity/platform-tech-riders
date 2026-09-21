using TechRiders.Domain.Entities;

namespace TechRiders.Application.Interfaces;

/// <summary>Catalogo de skills (jerarquico) y gestion de las skills propias de un usuario.</summary>
public interface ISkillsService
{
    Task<List<Skill>> GetCatalogAsync(CancellationToken cancellationToken = default);
    Task<List<UserSkill>> GetUserSkillsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserSkill> AddOrUpdateAsync(Guid userId, Guid skillId, string level, bool isSpeakerSkill, bool isMentorSkill, CancellationToken cancellationToken = default);
    Task RemoveAsync(Guid userId, Guid skillId, CancellationToken cancellationToken = default);
}
