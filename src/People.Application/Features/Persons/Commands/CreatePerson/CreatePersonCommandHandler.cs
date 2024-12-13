using MediatR;
using People.Application.Features.Persons.Dtos;
using People.Application.Models;
using People.Application.Repositories.Common;

namespace People.Application.Features.Persons.Commands.CreatePerson;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, ApiResponse<PersonDetailsDto>>
{
    private readonly IRepository<Domain.Entities.Person> _personRepository;

    public CreatePersonCommandHandler(IRepository<Domain.Entities.Person> personRepository)
    {
        _personRepository = personRepository;
    }
    
    public async Task<ApiResponse<PersonDetailsDto>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLower();
        
        if (await _personRepository.AnyAsync(x => x.Email == email, cancellationToken))
            return ApiResponse.Error<PersonDetailsDto>(ResponseCode.Found, "Exist person");
        
        var person = new Domain.Entities.Person
        {
            Fullname = request.Fullname,
            DateOfBirth = request.DateOfBirth,
            Email = email,
            PhoneNumber = request.PhoneNumber,
            Dni = request.Dni,
            CreatedAt = DateTime.UtcNow
        };
        
        _personRepository.Add(person);

        await _personRepository.SaveChangesAsync(cancellationToken);

        return ApiResponse.OkMapped<PersonDetailsDto>(person, ResponseCode.Created);
    }
}