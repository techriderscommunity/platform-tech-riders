using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using TechRiders.Api.Extensions;
using TechRiders.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// =====================================================================
// CONFIGURACIÓN DE SERVICIOS - Dependency Injection
// =====================================================================

// 1. Configuración de autenticación JWT con Microsoft Identity
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
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
builder.Services.AddHealthChecks()
    .AddDbContextCheck<TechRiders.Infrastructure.Data.TechRidersDbContext>();

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
