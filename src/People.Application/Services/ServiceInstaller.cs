using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using People.Application.Services.Persons;

namespace People.Application.Services;

public static class ServiceInstaller
{
    public static IServiceCollection InstallAplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPicturePersonService, PicturePersonService>();
        
        return services;
    }
}