using Mapster;
using TechRiders.Application.DTOs.Requests.Category;
using TechRiders.Application.DTOs.Responses.Category;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Mappings;

/// <summary>
/// Mapster configuration for Category domain
/// </summary>
public class CategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MT_Category, CategoryResponse>()
            .Map(dest => dest.FatherName, src => src.Main != null ? src.Main.Name : null)
            .Map(dest => dest.SubCategories, src => src.Secondary);

        config.NewConfig<CreateCategoryRequest, MT_Category>();

        config.NewConfig<UpdateCategoryRequest, MT_Category>()
            .Map(dest => dest, src => src, srcMember => srcMember != null);
    }
}
