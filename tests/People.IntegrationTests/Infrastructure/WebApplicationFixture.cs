using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using People.Application.Repositories.Common;
using People.Infrastructure.Persistence.Common;
using People.Infrastructure.Persistence.Context;
using People.Infrastructure.Persistence.Seeds;

namespace People.IntegrationTests.Infrastructure;

public class WebApplicationFixture(MySqlContainerFixture mySqlContainerFixture)
    : WebApplicationFactory<Program>, IAsyncLifetime
{

    async Task IAsyncLifetime.InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var dbSeed = scope.ServiceProvider.GetRequiredService<PeopleContextSeed>();
            
        await dbSeed.EnsureCreatedAsync();
        await dbSeed.SeedAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        using var scope = Services.CreateScope();
        var dbSeed = scope.ServiceProvider.GetRequiredService<PeopleContextSeed>();
        
        await dbSeed.EnsureDeletedAsync();
    }

    public PeopleContext DbContext => Services.GetRequiredService<PeopleContext>();
    public HttpClient HttpClient => CreateClient();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<PeopleContext>));

            services.Remove(descriptor!);

            var descriptorAppDbContext = services.SingleOrDefault(
                d => d.ServiceType == typeof(PeopleContext));

            services.Remove(descriptorAppDbContext!);

            services.AddDbContext<PeopleContext>(options =>
            {
                options.UseMySQL(mySqlContainerFixture.ConnectionString);
            });
            
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        });
    }
}