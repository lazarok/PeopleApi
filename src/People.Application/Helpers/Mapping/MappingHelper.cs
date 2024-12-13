using AutoMapper;

namespace People.Application.Helpers.Mapping;

public static class MappingHelper
{
    private static IMapper _mapper;
    
    public static void Configure(IMapper mapper)
    {
        _mapper = mapper;
    }

    public static T Map<T>(object source)
    {
        return _mapper.Map<T>(source);
    }
}