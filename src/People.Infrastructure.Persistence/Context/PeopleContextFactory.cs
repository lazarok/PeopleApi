using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace People.Infrastructure.Persistence.Context;

public class PeopleContextFactory : IDesignTimeDbContextFactory<PeopleContext>
{
    public PeopleContext CreateDbContext(string[] args)
    {
        // Get environment
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // Build config
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../People.Api"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        // Get connection string
        var optionsBuilder = new DbContextOptionsBuilder<PeopleContext>();
        
        var connectionString = configuration.GetConnectionString("People")!;
        
        optionsBuilder.UseMySQL(connectionString, b => b.MigrationsAssembly(typeof(PeopleContext).Assembly.FullName));
        
        var context = new PeopleContext(optionsBuilder.Options);
        
        return context;
    }
}