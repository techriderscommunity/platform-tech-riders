using Mapster;
using MapsterMapper;
using TechRiders.Application.Interfaces;
using TechRiders.Application.Services;

namespace TechRiders.Api.Extensions;

/// <summary>
/// Métodos de extensión para configurar servicios de aplicación
/// </summary>
public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Registra servicios de la capa de aplicación
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar Mapster con todos los perfiles del assembly de Application
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ApplicationServiceExtensions).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        // Registrar servicios de aplicación
        services.AddScoped<IAmbassadorService, AmbassadorService>();
        services.AddScoped<IAdminDashboardService, AdminDashboardService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICenterService, CenterService>();
        services.AddScoped<IEmploymentService, EmploymentService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IFPTourService, FPTourService>();
        services.AddScoped<IIntranetService, IntranetService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ITutorialsService, TutorialsService>();

        return services;
    }
}
