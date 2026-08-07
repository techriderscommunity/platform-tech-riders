using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using TechRiders.Application.Interfaces;
using TechRiders.Application.Mappings;
using TechRiders.Application.Services;

namespace TechRiders.Application.Extensions;

/// <summary>
/// Extension methods for registering application layer services
/// </summary>
public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Adds application services to the dependency injection container
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register domain services
        services.AddScoped<IEmpleoService, EmpleoService>();
        services.AddScoped<ITutorialesService, TutorialesService>();
        services.AddScoped<IIntranetService, IntranetService>();

        // Register Mapster configurations

        var config = TypeAdapterConfig.GlobalSettings;

        config.Scan(typeof(ApplicationServiceExtensions).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();


        return services;
    }
}
