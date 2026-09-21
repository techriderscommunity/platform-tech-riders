using Microsoft.EntityFrameworkCore;
using TechRiders.Application.KnowledgeArticles;
using TechRiders.Domain.Entities;
using TechRiders.Infrastructure.Data;

namespace TechRiders.Api.Services;

/// <summary>
/// Seed idempotente de los prerequisitos de KnowledgeArticle: usuario de migraci\u00f3n,
/// Status "Published" y categor\u00eda fallback "Sin categor\u00eda". Sin estos registros el
/// import de tutoriales de WordPress se rechaza expl\u00edcitamente (ver KnowledgeArticleService).
/// </summary>
public static class KnowledgeArticleSeedService
{
    public static async Task EnsureDefaultsAsync(TechRidersDbContext dbContext, ILogger logger, CancellationToken cancellationToken = default)
    {
        var migrationUser = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == KnowledgeArticleDefaults.MigrationAuthorEmail, cancellationToken);

        if (migrationUser is null)
        {
            migrationUser = new User
            {
                Id = Guid.NewGuid(),
                Nickname = "wp-import",
                Name = "Migraci\u00f3n",
                LastName = "WordPress",
                Email = KnowledgeArticleDefaults.MigrationAuthorEmail,
                IsWorking = false,
                About = "Cuenta t\u00e9cnica usada como autor de los tutoriales hist\u00f3ricos migrados desde WordPress."
            };
            await dbContext.Users.AddAsync(migrationUser, cancellationToken);
        }

        var publishedStatus = await dbContext.Set<Status>()
            .FirstOrDefaultAsync(s => s.Scope == KnowledgeArticleDefaults.StatusScope && s.Name == KnowledgeArticleDefaults.PublishedStatusName, cancellationToken);

        if (publishedStatus is null)
        {
            await dbContext.Set<Status>().AddAsync(new Status
            {
                Id = Guid.NewGuid(),
                Name = KnowledgeArticleDefaults.PublishedStatusName,
                Scope = KnowledgeArticleDefaults.StatusScope
            }, cancellationToken);
        }

        var fallbackCategory = await dbContext.Set<Category>()
            .FirstOrDefaultAsync(c => c.Name == KnowledgeArticleDefaults.FallbackCategoryName, cancellationToken);

        if (fallbackCategory is null)
        {
            fallbackCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = KnowledgeArticleDefaults.FallbackCategoryName,
                Description = "Categoría por defecto para contenido migrado sin mapeo de categoría claro."
            };
            await dbContext.Set<Category>().AddAsync(fallbackCategory, cancellationToken);
        }

        // 1. Seed master categories into Category (Guid) table
        var mtCategories = await dbContext.Set<MT_Category>().Where(c => c.Active).ToListAsync(cancellationToken);
        var categoryMap = new Dictionary<string, Category>(StringComparer.OrdinalIgnoreCase);

        foreach (var mtCat in mtCategories)
        {
            var cat = await dbContext.Set<Category>().FirstOrDefaultAsync(c => c.Name == mtCat.Name, cancellationToken);
            if (cat is null)
            {
                cat = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = mtCat.Name,
                    IsActive = true
                };
                await dbContext.Set<Category>().AddAsync(cat, cancellationToken);
            }
            categoryMap[mtCat.Name] = cat;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        // 2. Classify articles that currently only have "Sin categoría"
        var unassignedArticles = await dbContext.Set<KnowledgeArticle>()
            .Include(a => a.Categories)
            .Where(a => a.Categories.All(c => c.CategoryId == fallbackCategory.Id))
            .ToListAsync(cancellationToken);

        if (unassignedArticles.Count > 0)
        {
            foreach (var article in unassignedArticles)
            {
                var title = article.Title.ToLowerInvariant();
                var slug = article.Slug.ToLowerInvariant();
                var text = $"{title} {slug}";

                var matchedCategoryNames = new List<string>();

                if (text.Contains("angular") || text.Contains("frontend") || text.Contains("react") || text.Contains("vue") || text.Contains("css") || text.Contains("html") || text.Contains("web"))
                {
                    matchedCategoryNames.Add("Programación Frontend");
                }
                if (text.Contains("asp.net") || text.Contains("backend") || text.Contains("c#") || text.Contains(".net") || text.Contains("java") || text.Contains("python") || text.Contains("api") || text.Contains("jwt") || text.Contains("sql"))
                {
                    matchedCategoryNames.Add("Programación Backend");
                }
                if (text.Contains("power automate") || text.Contains("devops") || text.Contains("docker") || text.Contains("kubernetes") || text.Contains("automatiz") || text.Contains("ci/cd"))
                {
                    matchedCategoryNames.Add("DevOps y Automatización");
                }
                if (text.Contains("cloud") || text.Contains("azure") || text.Contains("aws") || text.Contains("gcp"))
                {
                    matchedCategoryNames.Add("Cloud Computing");
                }
                if (text.Contains("ia") || text.Contains("inteligencia artificial") || text.Contains("data") || text.Contains("big data") || text.Contains("power bi") || text.Contains("machine learning"))
                {
                    matchedCategoryNames.Add("Inteligencia Artificial Aplicada");
                }
                if (text.Contains("seguridad") || text.Contains("hacking") || text.Contains("ciberseguridad"))
                {
                    matchedCategoryNames.Add("Ciberseguridad y Hacking Ético");
                }

                if (matchedCategoryNames.Count > 0)
                {
                    article.Categories.Clear();
                    foreach (var catName in matchedCategoryNames)
                    {
                        if (categoryMap.TryGetValue(catName, out var targetCategory))
                        {
                            article.Categories.Add(new KnowledgeArticleCategory
                            {
                                KnowledgeArticleId = article.Id,
                                CategoryId = targetCategory.Id
                            });
                        }
                    }
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Reclassified {Count} articles with real master categories.", unassignedArticles.Count);
        }

        logger.LogInformation("KnowledgeArticle defaults ensured (migration user, Published status, fallback category).");
    }
}
