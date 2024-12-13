using System.Reflection;
using AutoMapper;
using People.Application;

namespace People.MapperTests;

public class MapperConfigTests
{

    [Fact]
    public void TestAll()
    {
        var assemblies = new List<Assembly>()
        {
            typeof(IApplicationMarker).Assembly
        };

        var profiles = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(Profile).IsAssignableFrom(t))
            .Where(t => !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<Profile>()
            .ToList();

        var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
        
        configuration.AssertConfigurationIsValid();
    }
}