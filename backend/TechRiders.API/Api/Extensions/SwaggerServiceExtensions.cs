using Microsoft.OpenApi;

namespace TechRiders.Api.Extensions;

/// <summary>
/// Métodos de extensión para configurar Swagger mejorado
/// </summary>
public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TechRiders API",
                Version = "v1",
                Description = "API para gestión de eventos y sesiones"
            });

            options.EnableAnnotations();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Introduce el token JWT. Ejemplo: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(_ =>
            {
                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(
                    new OpenApiSecuritySchemeReference("Bearer", null, null),
                    new List<string>());

                return securityRequirement;
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "TechRiders API v1");
            options.DisplayRequestDuration();
            options.EnablePersistAuthorization();
            options.InjectJavascript("/swagger/swagger-auto-auth.js");
        });

        return app;
    }
}
