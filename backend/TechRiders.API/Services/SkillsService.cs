using Microsoft.EntityFrameworkCore;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>Cat\u00e1logo de skills (jer\u00e1rquico) y gesti\u00f3n de las skills propias de un usuario.</summary>
public static class SkillsService
{
    public static Task<List<Skill>> GetCatalogAsync(TechRidersDbContext dbContext, CancellationToken cancellationToken = default) =>
        dbContext.Set<Skill>().Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync(cancellationToken);

    public static Task<List<UserSkill>> GetUserSkillsAsync(TechRidersDbContext dbContext, Guid userId, CancellationToken cancellationToken = default) =>
        dbContext.Set<UserSkill>()
            .Include(us => us.Skill)
            .Where(us => us.UserId == userId)
            .ToListAsync(cancellationToken);

    public static async Task<UserSkill> AddOrUpdateAsync(
        TechRidersDbContext dbContext, Guid userId, Guid skillId, string level,
        bool isSpeakerSkill, bool isMentorSkill, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<SkillLevel>(level, ignoreCase: true, out var parsedLevel))
        {
            throw new ArgumentException($"Nivel '{level}' no reconocido.");
        }

        var skillExists = await dbContext.Set<Skill>().AnyAsync(s => s.Id == skillId, cancellationToken);
        if (!skillExists)
        {
            throw new InvalidOperationException("La skill indicada no existe.");
        }

        var userSkill = await dbContext.Set<UserSkill>().FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId, cancellationToken);
        if (userSkill is null)
        {
            userSkill = new UserSkill { UserId = userId, SkillId = skillId };
            await dbContext.Set<UserSkill>().AddAsync(userSkill, cancellationToken);
        }

        userSkill.Level = parsedLevel;
        userSkill.IsSpeakerSkill = isSpeakerSkill;
        userSkill.IsMentorSkill = isMentorSkill;

        await dbContext.SaveChangesAsync(cancellationToken);
        return userSkill;
    }

    public static async Task RemoveAsync(TechRidersDbContext dbContext, Guid userId, Guid skillId, CancellationToken cancellationToken = default)
    {
        var userSkill = await dbContext.Set<UserSkill>().FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId, cancellationToken)
            ?? throw new InvalidOperationException("El usuario no tiene esa skill registrada.");

        dbContext.Set<UserSkill>().Remove(userSkill);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
