using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechRiders.Application.Interfaces;
using TechRiders.Domain.Interfaces;
using TechRiders.Infrastructure.Data;
using TechRiders.Infrastructure.Repositories;
using TechRiders.Infrastructure.Storage;

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
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found in configuration.");

        services.AddDbContext<TechRidersDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
            });
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<KnowledgeStorageOptions>(configuration.GetSection(KnowledgeStorageOptions.SectionName));
        services.AddScoped<IKnowledgeContentBlobService, KnowledgeContentBlobService>();
        services.AddScoped<IProfileMediaService, ProfileMediaService>();

        services.AddScoped<IApprovalsService, Services.ApprovalsService>();
        services.AddScoped<IAssignmentService, Services.AssignmentService>();
        services.AddScoped<IConsentService, Services.ConsentService>();
        services.AddScoped<IGpfPersonLinkService, Services.GpfPersonLinkService>();
        services.AddScoped<IPreferenceService, Services.PreferenceService>();
        services.AddScoped<IPrivacyRequestService, Services.PrivacyRequestService>();
        services.AddScoped<IProfileVisibilityService, Services.ProfileVisibilityService>();
        services.AddScoped<ISkillsService, Services.SkillsService>();
        services.AddScoped<IOrganizationService, Services.OrganizationService>();
        services.AddScoped<ICapabilityRequestService, Services.CapabilityRequestService>();
        services.AddScoped<IEventSessionOpsService, Services.EventSessionOpsService>();
        services.AddScoped<IPasswordHasher, Services.PasswordHasher>();
        services.AddScoped<IAuthService, Services.AuthService>();
        services.AddScoped<IUserAdminService, Services.UserAdminService>();

        return services;
    }
}
