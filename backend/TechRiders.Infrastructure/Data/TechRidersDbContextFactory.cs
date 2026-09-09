using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TechRiders.Infrastructure.Data;

public sealed class TechRidersDbContextFactory : IDesignTimeDbContextFactory<TechRidersDbContext>
{
    public TechRidersDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TechRidersDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("TECHRIDERS_MIGRATION_CONNECTION")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=TechRidersDev;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

        optionsBuilder.UseSqlServer(connectionString);
        return new TechRidersDbContext(optionsBuilder.Options);
    }
}