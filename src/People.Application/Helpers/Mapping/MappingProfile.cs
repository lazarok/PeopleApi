using System.Reflection;
using AutoMapper;
using People.Application.Models;

namespace People.Application.Helpers.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(typeof(MappingProfile).Assembly);
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IMapping))).ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            if(instance != null)
                ((IMapping)instance).CreateMap(this);
        }
        
        CreateMap(typeof(PagedList<>), typeof(PagedList<>));
    }
}