using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TechRiders.Api.Extensions;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Data;
using TechRiders.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

const string AzureAdScheme = "AzureAd";
const string LocalJwtScheme = JwtBearerDefaults.AuthenticationScheme;

// =====================================================================
// CONFIGURACIÓN DE SERVICIOS - Dependency Injection
// =====================================================================

var localJwtSigningKey = builder.Configuration["LocalAuth:SigningKey"] ?? "techriders-local-auth-signing-key-2025";

var authenticationBuilder = builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = LocalJwtScheme;
    options.DefaultAuthenticateScheme = LocalJwtScheme;
    options.DefaultChallengeScheme = LocalJwtScheme;
});

authenticationBuilder
    .AddJwtBearer(LocalJwtScheme, options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["LocalAuth:Issuer"] ?? "TechRidersLocalAuth",
            ValidAudience = builder.Configuration["LocalAuth:Audience"] ?? "TechRidersApi",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(localJwtSigningKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    })
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"), jwtBearerScheme: AzureAdScheme)
        .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();

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
var useInMemoryDatabase = bool.TryParse(builder.Configuration["Database:UseInMemory"], out var useInMemoryParsed)
    && useInMemoryParsed;
var healthChecks = builder.Services.AddHealthChecks();
if (!useInMemoryDatabase)
{
    healthChecks.AddDbContextCheck<TechRiders.Infrastructure.Data.TechRidersDbContext>();
}

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
    app.UseExceptionHandler("/error");
    app.UseHsts(); // HTTP Strict Transport Security
}

// 2. Swagger - Disponible en todos los entornos
app.UseSwaggerDocumentation();

// 3. Redirección HTTPS
app.UseHttpsRedirection();

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

    if (dbContext.Database.IsRelational())
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

    await LocalAuthService.EnsureDefaultAdminAsync(dbContext, configuration, logger);
}

// Logging de inicio
app.Logger.LogInformation("TechRiders API iniciando...");
app.Logger.LogInformation("Entorno: {Environment}", app.Environment.EnvironmentName);
app.Logger.LogInformation("Swagger UI disponible en: /swagger");

app.Run();

public partial class Program { }
