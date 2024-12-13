using MediatR;
using People.Application.Models;
using People.Application.Repositories.Common;

namespace People.Application.Features.Persons.Commands.DeletePerson;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, ApiResponse>
{
    private readonly IRepository<Domain.Entities.Person> _personRepository;

    public DeletePersonCommandHandler(IRepository<Domain.Entities.Person> personRepository)
    {
        _personRepository = personRepository;
    }
    
    public async Task<ApiResponse> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if(person is null)
            return ApiResponse.Error(ResponseCode.NotFound, "Person not found");
        
        _personRepository.Remove(person);

        await _personRepository.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok();
    }
}