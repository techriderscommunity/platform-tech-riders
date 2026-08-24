using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;
using TechRiders.Infrastructure.Repositories;

namespace TechRiders.Infrastructure.Extensions;

/// <summary>
/// Extensiones para registrar los servicios y dependencias de infraestructura.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registra el contexto de datos y los servicios de persistencia de la infraestructura.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var useInMemoryDatabase = bool.TryParse(configuration["Database:UseInMemory"], out var parsedValue)
            && parsedValue;

        if (useInMemoryDatabase)
        {
            services.AddDbContext<TechRidersDbContext>(options =>
            {
                options.UseInMemoryDatabase("TechRidersDb");
            });
        }
        else
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found in configuration.");

            services.AddDbContext<TechRidersDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
