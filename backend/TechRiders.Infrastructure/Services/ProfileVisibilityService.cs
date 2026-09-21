using Microsoft.EntityFrameworkCore;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Entities;
using TechRiders.Domain.Enums;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Infrastructure.Services;

/// <summary>Visibilidad de campos del perfil (Requisitos Arquitectura §10). Por defecto Private hasta configuracion explicita.</summary>
public sealed class ProfileVisibilityService : IProfileVisibilityService
{
    private readonly TechRidersDbContext _dbContext;

    public ProfileVisibilityService(TechRidersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<UserFieldVisibility>> GetForUserAsync(Guid userId, CancellationToken cancellationToken = default) =>
        _dbContext.Set<UserFieldVisibility>().Where(v => v.UserId == userId).ToListAsync(cancellationToken);

    public async Task<UserFieldVisibility> SetAsync(Guid userId, string fieldKey, string visibility, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<FieldVisibility>(visibility, ignoreCase: true, out var parsedVisibility))
        {
            throw new ArgumentException($"Visibilidad '{visibility}' no reconocida.");
        }

        var entry = await _dbContext.Set<UserFieldVisibility>()
            .FirstOrDefaultAsync(v => v.UserId == userId && v.FieldKey == fieldKey, cancellationToken);

        if (entry is null)
        {
            entry = new UserFieldVisibility { Id = Guid.NewGuid(), UserId = userId, FieldKey = fieldKey, Visibility = parsedVisibility };
            await _dbContext.Set<UserFieldVisibility>().AddAsync(entry, cancellationToken);
        }
        else
        {
            entry.Visibility = parsedVisibility;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return entry;
    }
}
