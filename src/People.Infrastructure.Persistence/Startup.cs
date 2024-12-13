﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using People.Application.Repositories.Common;
using People.Domain.Entities;
using People.Infrastructure.Persistence.Common;
using People.Infrastructure.Persistence.Context;
using People.Infrastructure.Persistence.Seeds;

namespace People.Infrastructure.Persistence;

public static class Startup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PeopleContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("People")!));
        
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        
        var container = services.BuildServiceProvider();
        
        // Run migrations
        var peopleContext = container.GetRequiredService<PeopleContext>();
        peopleContext.Database.Migrate();
        
        // Seed
        var repository = container.GetRequiredService<IRepository<Person>>();
        BuildMockPeople.Build(repository);
        
        return services;
    }
}