using MediatR;
using Microsoft.Extensions.Logging;
using People.Application.Features.Persons.Dtos;
using People.Application.Models;
using People.Application.Repositories.Common;

namespace People.Application.Features.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandHandler: IRequestHandler<UpdatePersonCommand, ApiResponse<PersonDetailsDto>>
{
    private readonly IRepository<Domain.Entities.Person> _personRepository;
    private readonly ILogger<UpdatePersonCommandHandler> _logger;

    public UpdatePersonCommandHandler(
        IRepository<Domain.Entities.Person> personRepository, 
        ILogger<UpdatePersonCommandHandler> logger) 
    {
        _personRepository = personRepository;
        _logger = logger;
    }
    
    public async Task<ApiResponse<PersonDetailsDto>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (person is null)
        {
            _logger.LogError("Person not found for Id: {Id}", request.Id);
            return ApiResponse.Error<PersonDetailsDto>(ResponseCode.NotFound, "Person not found");
        }

        person.Fullname = request.Fullname;
        person.DateOfBirth = request.DateOfBirth;
        person.PhoneNumber = request.PhoneNumber;
        person.Dni = request.Dni;
        
        _personRepository.Update(person);

        await _personRepository.SaveChangesAsync(cancellationToken);

        return ApiResponse.OkMapped<PersonDetailsDto>(person);
    }
}