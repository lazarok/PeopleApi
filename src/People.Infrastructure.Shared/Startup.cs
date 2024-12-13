using Amazon.S3;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using People.Application.Services;
using People.Infrastructure.Shared.Storage;

namespace People.Infrastructure.Shared;

public static class Startup
{
    public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // For AWS S3
        // services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        // services.AddAWSService<IAmazonS3>();
        // services.AddTransient<IStorageService, S3StorageService>();
        
        
        services.AddSingleton<IStorageService>(_ => new FileSystemStorageService(configuration["StoragePath"!]));
    }
}