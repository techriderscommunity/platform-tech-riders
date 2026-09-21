using Mapster;
using TechRiders.Application.DTOs.Responses.KnowledgeArticle;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Mappings;

public class KnowledgeArticleMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<KnowledgeArticle, KnowledgeArticleSummaryResponse>()
            .Map(dest => dest.AuthorName, src => src.Author != null ? $"{src.Author.Name} {src.Author.LastName}" : string.Empty)
            .Map(dest => dest.StatusName, src => src.Status != null ? src.Status.Name : null)
            .Map(dest => dest.Categories, src => src.Categories.Where(c => c.Category != null).Select(c => c.Category.Name))
            .Map(dest => dest.Skills, src => src.Skills.Where(s => s.Skill != null).Select(s => s.Skill.Name));

        config.NewConfig<KnowledgeArticle, KnowledgeArticleResponse>()
            .Map(dest => dest.AuthorName, src => src.Author != null ? $"{src.Author.Name} {src.Author.LastName}" : string.Empty)
            .Map(dest => dest.StatusName, src => src.Status != null ? src.Status.Name : null)
            .Map(dest => dest.Categories, src => src.Categories.Where(c => c.Category != null).Select(c => c.Category.Name))
            .Map(dest => dest.Skills, src => src.Skills.Where(s => s.Skill != null).Select(s => s.Skill.Name))
            .Ignore(dest => dest.ContentMd);
    }
}
