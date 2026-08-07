using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;
using TechRiders.Infrastructure.Repositories;

namespace TechRiders.Infrastructure.Extensions;

/// <summary>
/// Métodos de extensión para configurar servicios de infraestructura
/// Sigue el principio de Single Responsibility y Open/Closed
/// </summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Configura Entity Framework Core con DbContext Pooling para SQL Server
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var useInMemory = bool.TryParse(configuration["Database:UseInMemory"], out var useInMemoryParsed)
            && useInMemoryParsed;
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Configurar DbContext con pooling (mejora de rendimiento según EF Core 2.0+)
        services.AddDbContextPool<TechRidersDbContext>(options =>
        {
            if (useInMemory || string.IsNullOrWhiteSpace(connectionString))
            {
                // Fallback de desarrollo: permite avanzar sin acceso a SQL Server.
                options.UseInMemoryDatabase("TechRidersDevInMemory");
            }
            else
            {
                options.UseSqlServer(
                    connectionString,
                    sqlOptions =>
                    {
                        // Habilitar reintentos automáticos para resiliencia en Azure SQL
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);

                        // Mejorar rendimiento de consultas complejas
                        sqlOptions.CommandTimeout(30);
                    });
            }

            // Solo en desarrollo, mostrar queries sensibles
            // if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
            // {
            //     options.EnableSensitiveDataLogging();
            //     options.EnableDetailedErrors();
            // }
        }, poolSize: 128); // Pool size recomendado para aplicaciones de carga media

        // Registrar Unit of Work y Repositorios
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAmbassadorRepository, AmbassadorRepository>();
        services.AddScoped<ICandidaturaRepository, CandidaturaRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICenterRepository, CenterRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IFPTourRepository, FPTourRepository>();
        services.AddScoped<IIntranetAuditLogRepository, IntranetAuditLogRepository>();
        services.AddScoped<IIntranetSettingRepository, IntranetSettingRepository>();
        services.AddScoped<IIntranetUserCategoryRepository, IntranetUserCategoryRepository>();
        services.AddScoped<IOfertaRepository, OfertaRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ITutorialRepository, TutorialRepository>();
        

        return services;
    }
}
