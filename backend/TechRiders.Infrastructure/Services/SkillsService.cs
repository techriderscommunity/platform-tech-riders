using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>Catalogo de skills (jerarquico) y gestion de las skills propias de un usuario.</summary>
public sealed class SkillsService : ISkillsService
{
    private readonly TechRidersDbContext _dbContext;

    public SkillsService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Skill>> GetCatalogAsync(CancellationToken cancellationToken = default) =>
        _dbContext.Set<Skill>().Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync(cancellationToken);

    public Task<List<UserSkill>> GetUserSkillsAsync(Guid userId, CancellationToken cancellationToken = default) =>
        _dbContext.Set<UserSkill>()
            .Include(us => us.Skill)
            .Where(us => us.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<UserSkill> AddOrUpdateAsync(
        Guid userId, Guid skillId, string level,
        bool isSpeakerSkill, bool isMentorSkill, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<SkillLevel>(level, ignoreCase: true, out var parsedLevel))
        {
            throw new ArgumentException($"Nivel '{level}' no reconocido.");
        }

        var skillExists = await _dbContext.Set<Skill>().AnyAsync(s => s.Id == skillId, cancellationToken);
        if (!skillExists)
        {
            throw new InvalidOperationException("La skill indicada no existe.");
        }

        var userSkill = await _dbContext.Set<UserSkill>().FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId, cancellationToken);
        if (userSkill is null)
        {
            userSkill = new UserSkill { UserId = userId, SkillId = skillId };
            await _dbContext.Set<UserSkill>().AddAsync(userSkill, cancellationToken);
        }

        userSkill.Level = parsedLevel;
        userSkill.IsSpeakerSkill = isSpeakerSkill;
        userSkill.IsMentorSkill = isMentorSkill;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return userSkill;
    }

    public async Task RemoveAsync(Guid userId, Guid skillId, CancellationToken cancellationToken = default)
    {
        var userSkill = await _dbContext.Set<UserSkill>().FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId, cancellationToken)
            ?? throw new InvalidOperationException("El usuario no tiene esa skill registrada.");

        _dbContext.Set<UserSkill>().Remove(userSkill);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
