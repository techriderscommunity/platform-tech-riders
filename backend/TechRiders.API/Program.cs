using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TechRiders.Api.Extensions;
using TechRiders.Api.Services;
using TechRiders.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

const string DynamicAuthScheme = "DynamicJwt";
const string AzureAdScheme = "AzureAd";
const string LocalJwtScheme = "LocalJwt";

// =====================================================================
// CONFIGURACIÓN DE SERVICIOS - Dependency Injection
// =====================================================================

// 1. Configuración de autenticación JWT con Microsoft Identity y JWT local para desarrollo.
var localJwtKey = builder.Configuration["JWT_KEY"]
    ?? builder.Configuration[$"{LocalAuthOptions.SectionName}:JwtKey"];

if (string.IsNullOrWhiteSpace(localJwtKey))
{
    throw new InvalidOperationException("JWT key for local auth is required. Configure JWT_KEY or LocalAuth:JwtKey.");
}

builder.Services.Configure<LocalAuthOptions>(builder.Configuration.GetSection(LocalAuthOptions.SectionName));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = DynamicAuthScheme;
    options.DefaultAuthenticateScheme = DynamicAuthScheme;
    options.DefaultChallengeScheme = DynamicAuthScheme;
})
    .AddPolicyScheme(DynamicAuthScheme, "Azure AD o JWT local", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            var authorization = context.Request.Headers.Authorization.ToString();
            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authorization["Bearer ".Length..].Trim();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var jwtHandler = new JwtSecurityTokenHandler();
                    if (jwtHandler.CanReadToken(token))
                    {
                        var parsedToken = jwtHandler.ReadJwtToken(token);
                        if (string.Equals(parsedToken.Issuer, LocalAuthOptions.DefaultIssuer, StringComparison.OrdinalIgnoreCase))
                        {
                            return LocalJwtScheme;
                        }
                    }
                }
            }

            return AzureAdScheme;
        };
    })
    .AddJwtBearer(LocalJwtScheme, options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = LocalAuthOptions.DefaultIssuer,
            ValidateAudience = true,
            ValidAudience = LocalAuthOptions.DefaultAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(localJwtKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2),
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

// 3.1 Estado MVP en memoria para flujos locales mientras no exista persistencia definitiva.
builder.Services.AddSingleton<IMvpRuntimeStateStore, InMemoryMvpRuntimeStateStore>();

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

// Logging de inicio
app.Logger.LogInformation("TechRiders API iniciando...");
app.Logger.LogInformation("Entorno: {Environment}", app.Environment.EnvironmentName);
app.Logger.LogInformation("Swagger UI disponible en: /swagger");

app.Run();
