using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechRiders.Api.Contracts.Requests.Auth;
using TechRiders.Api.Contracts.Responses.Auth;
using TechRiders.Infrastructure.Data;
using Xunit;

namespace TechRiders.Tests;

public sealed class AuthFlowIntegrationTests : IClassFixture<AuthApiFactory>
{
    private readonly AuthApiFactory _factory;

    public AuthFlowIntegrationTests(AuthApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Local_admin_seed_should_create_admin_role_and_allow_login_with_test_credentials()
    {
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        const string email = "shierroc@gmail.com";
        const string password = "TechAdmin";

        var loginResponse = await client.PostAsJsonAsync("/api/Auth/login", new LoginRequest
        {
            Email = email,
            Password = password
        });

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LocalLoginResponse>();
        Assert.NotNull(loginResult);
        Assert.False(string.IsNullOrWhiteSpace(loginResult!.Token));
        Assert.Equal("admin", loginResult.User.Role);

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TechRidersDbContext>();
        var user = await dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleAsync(u => u.Email == email);

        Assert.NotNull(user.PasswordHash);
        Assert.Contains(user.UserRoles, ur => string.Equals(ur.Role.Name, "Admin", StringComparison.OrdinalIgnoreCase));
        Assert.True(TechRiders.Api.Services.LocalAuthService.VerifyPassword(password, user.PasswordHash!));
    }

    [Fact]
    public async Task Register_login_forgot_password_and_reset_password_should_work_end_to_end()
    {
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        const string email = "maria.perez@techriders.test";
        const string oldPassword = "Password123!";
        const string newPassword = "Password456!";

        var registerRequest = new RegisterRequest
        {
            Nickname = "maria",
            Name = "María",
            LastName = "Pérez",
            Email = email,
            Password = oldPassword
        };

        var registerResponse = await client.PostAsJsonAsync("/api/Auth/register", registerRequest);
        Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);
        var registerResult = await registerResponse.Content.ReadFromJsonAsync<RegisterResponse>();
        Assert.NotNull(registerResult);
        Assert.Equal(email, registerResult!.Email);

        var loginResponse = await client.PostAsJsonAsync("/api/Auth/login", new LoginRequest
        {
            Email = email,
            Password = oldPassword
        });

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LocalLoginResponse>();
        Assert.NotNull(loginResult);
        Assert.False(string.IsNullOrWhiteSpace(loginResult!.Token));

        var forgotResponse = await client.PostAsJsonAsync("/api/Auth/forgot-password", new ForgotPasswordRequest
        {
            Email = email
        });

        Assert.Equal(HttpStatusCode.OK, forgotResponse.StatusCode);
        var forgotResult = await forgotResponse.Content.ReadFromJsonAsync<ForgotPasswordResponse>();
        Assert.NotNull(forgotResult);
        Assert.False(string.IsNullOrWhiteSpace(forgotResult!.Token));

        var resetResponse = await client.PostAsJsonAsync("/api/Auth/reset-password", new ResetPasswordRequest
        {
            Email = email,
            Token = forgotResult.Token,
            NewPassword = newPassword
        });

        Assert.Equal(HttpStatusCode.OK, resetResponse.StatusCode);

        var oldPasswordLoginResponse = await client.PostAsJsonAsync("/api/Auth/login", new LoginRequest
        {
            Email = email,
            Password = oldPassword
        });
        Assert.Equal(HttpStatusCode.Unauthorized, oldPasswordLoginResponse.StatusCode);

        var newPasswordLoginResponse = await client.PostAsJsonAsync("/api/Auth/login", new LoginRequest
        {
            Email = email,
            Password = newPassword
        });
        Assert.Equal(HttpStatusCode.OK, newPasswordLoginResponse.StatusCode);
    }
}

public sealed class AuthApiFactory : WebApplicationFactory<Program>
{
    private static readonly string DatabaseName = $"TechRidersAuthFlowTests_{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        var connectionString = $"Server=(localdb)\\MSSQLLocalDB;Database={DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = connectionString,
                ["Database:UseInMemory"] = "false"
            });
        });

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TechRidersDbContext>));
            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            var contextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TechRidersDbContext));
            if (contextDescriptor is not null)
            {
                services.Remove(contextDescriptor);
            }

            services.AddDbContext<TechRidersDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TechRidersDbContext>();

            if (dbContext.Database.CanConnect())
            {
                dbContext.Database.EnsureDeleted();
            }

            dbContext.Database.Migrate();
        });
    }
}
