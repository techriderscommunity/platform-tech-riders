using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TechRiders.Api.Authorization;
using TechRiders.Api.Extensions;
using TechRiders.Api.Services;
using TechRiders.Application.Interfaces;
using TechRiders.Infrastructure.Data;
using TechRiders.Infrastructure.Extensions;
using TechRiders.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// =====================================================================
// CONFIGURACIÓN DE SERVICIOS - Dependency Injection
// =====================================================================

var authSection = builder.Configuration.GetSection("Auth");
var jwtSigningKey = authSection["SigningKey"];
if (string.IsNullOrWhiteSpace(jwtSigningKey))
{
    throw new InvalidOperationException("Auth:SigningKey must be configured.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authSection["Issuer"] ?? "TechRidersAuth",
            ValidAudience = authSection["Audience"] ?? "TechRidersApi",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddAuthorization();

// 2. Configuración de servicios de infraestructura (DbContext con pooling, repositorios)
builder.Services.AddInfrastructureServices(builder.Configuration);

// 3. Configuración de servicios de aplicación (servicios de negocio, Mapster)
builder.Services.AddApplicationServices();

// 4. Configuración de Controllers con validación de modelo automática
builder.Services.AddControllers(options =>
{
    // Deshabilitar validación automática para manejarla manualmente si es necesario
    // options.SuppressModelStateInvalidFilter = false;
})
.AddJsonOptions(options =>
{
    // Configuración JSON para APIs
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Mantener PascalCase
    options.JsonSerializerOptions.WriteIndented = true; // JSON legible en desarrollo
});

// 5. Configuración de CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [])
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// 6. Configuración de Swagger mejorado con documentación completa
builder.Services.AddSwaggerDocumentation();

// 7. Configuración de Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<TechRidersDbContext>();

// 8. Configuración de compresión de respuestas
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// 9. Configuración de caché
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

// =====================================================================
// CONSTRUCCIÓN DE LA APLICACIÓN
// =====================================================================

var app = builder.Build();

// =====================================================================
// CONFIGURACIÓN DEL PIPELINE HTTP - Middleware
// =====================================================================

// 1. Manejo de excepciones y errores
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var problem = new
            {
                success = false,
                message = "Se produjo un error inesperado en el servidor.",
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsJsonAsync(problem);
        });
    });
    app.UseHsts(); // HTTP Strict Transport Security
}

// 2. Swagger - Disponible en todos los entornos
app.UseSwaggerDocumentation();

// 3. Redirección HTTPS
// En localhost y perfiles de desarrollo/pruebas hay que evitar un 307 forzado
// porque la app frontend y la API se ejecutan con HTTP local en el mismo entorno.
if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}

// 4. Archivos estáticos (para custom CSS de Swagger)
app.UseStaticFiles();

// 5. Routing
app.UseRouting();

// 6. CORS - Debe ir después de UseRouting y antes de UseAuthorization
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "Production");

// 7. Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

// 8. Compresión y caché de respuestas
app.UseResponseCompression();
app.UseResponseCaching();

// 9. Mapeo de endpoints
app.MapControllers();

// 10. Health checks endpoint
app.MapHealthChecks("/health");

// =====================================================================
// INICIALIZACIÓN Y EJECUCIÓN
// =====================================================================

// Inicialización de la base de datos al arrancar la API.
// Este proyecto usa un flujo code-first con Entity Framework Core: el modelo C# define el esquema
// y todas las bases relacionales se gestionan con migraciones.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TechRidersDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");

    if (app.Environment.IsEnvironment("Testing"))
    {
        dbContext.Database.EnsureCreated();
        app.Logger.LogInformation("Base de datos de test creada con EnsureCreated().");
    }
    else if (dbContext.Database.IsRelational())
    {
        var canConnect = dbContext.Database.CanConnect();
        if (!canConnect)
        {
            dbContext.Database.Migrate();
            app.Logger.LogInformation("La base de datos relacional no existía y se aplicaron las migraciones pendientes.");
        }
        else
        {
            dbContext.Database.Migrate();
            app.Logger.LogInformation("Base de datos relacional detectada, conectada y validada con migraciones code-first.");
        }
    }
    else
    {
        dbContext.Database.EnsureCreated();
        app.Logger.LogInformation("Base de datos en memoria creada con EnsureCreated().");
    }

    await IdentityCatalogSeedService.EnsureDefaultsAsync(dbContext, logger);
    await PreferenceCatalogSeedService.EnsureDefaultsAsync(dbContext, logger);
    await RolePermissionCatalogSeedService.EnsureDefaultsAsync(dbContext, logger);
    await scope.ServiceProvider.GetRequiredService<IAuthService>().EnsureDefaultAdminAsync();
    await CommunitySeedService.EnsureDefaultsAsync(dbContext, logger);
    await EventSeedService.EnsureDefaultsAsync(dbContext, logger);
    await KnowledgeArticleSeedService.EnsureDefaultsAsync(dbContext, logger);
}

// Logging de inicio
app.Logger.LogInformation("TechRiders API iniciando...");
app.Logger.LogInformation("Entorno: {Environment}", app.Environment.EnvironmentName);
app.Logger.LogInformation("Swagger UI disponible en: /swagger");

app.Run();

public partial class Program { }
