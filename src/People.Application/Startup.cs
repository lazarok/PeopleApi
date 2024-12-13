using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using People.Application.Behaviours;
using People.Application.Helpers.Mapping;
using People.Application.Services;

namespace People.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfile()); }).CreateMapper());
        var provider = services.BuildServiceProvider();
        MappingHelper.Configure(provider.GetService<IMapper>()!);
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        
        services.InstallAplicationServices(configuration);

        return services;
    }
}