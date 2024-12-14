using MediatR;
using Microsoft.Extensions.Logging;
using People.Application.Models;
using People.Application.Repositories.Common;

namespace People.Application.Features.Persons.Commands.DeletePerson;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, ApiResponse>
{
    private readonly IRepository<Domain.Entities.Person> _personRepository;
    private readonly ILogger<DeletePersonCommandHandler> _logger;

    public DeletePersonCommandHandler(
        IRepository<Domain.Entities.Person> personRepository, 
        ILogger<DeletePersonCommandHandler> logger)
    {
        _personRepository = personRepository;
        _logger = logger;
    }
    
    public async Task<ApiResponse> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (person is null)
        {
            _logger.LogError("Person not found for Id: {Id}", request.Id);
            return ApiResponse.Error(ResponseCode.NotFound, "Person not found");
        }
        
        _personRepository.Remove(person);

        await _personRepository.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok();
    }
}