using People.Application.Features.Persons.Commands.UpdatePerson;
using People.Application.Features.Persons.Dtos;
using People.Application.Helpers.Mapping;

namespace People.Application.Features.Persons;

public class PersonMapper : IMapping
{
    public void CreateMap(MappingProfile profile)
    {
        profile.CreateMap<Domain.Entities.Person, PersonDetailsDto>()
            .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());
        
        
        profile.CreateMap<Domain.Entities.Person, PersonListDto>();

        profile.CreateMap<(UpdatePersonRequest Request, long PersonId), UpdatePersonCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PersonId))
            .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.Request.Fullname))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Request.DateOfBirth))
            .ForMember(dest => dest.Dni, opt => opt.MapFrom(src => src.Request.Dni))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Request.PhoneNumber));
    }
}