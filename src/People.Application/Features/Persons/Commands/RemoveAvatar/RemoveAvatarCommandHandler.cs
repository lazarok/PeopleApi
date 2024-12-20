using MediatR;
using Microsoft.Extensions.Logging;
using People.Application.Models;
using People.Application.Repositories.Common;
using People.Application.Services.Persons;
using People.Domain.Entities;

namespace People.Application.Features.Persons.Commands.RemoveAvatar;

public class RemoveAvatarCommandHandler : IRequestHandler<RemoveAvatarCommand, ApiResponse>
{
    private readonly IRepository<Person> _personRepository;
    private readonly IPicturePersonService _picturePersonService;
    private readonly ILogger<RemoveAvatarCommandHandler> _logger;

    public RemoveAvatarCommandHandler(
        IRepository<Person> personRepository, 
        IPicturePersonService picturePersonService, 
        ILogger<RemoveAvatarCommandHandler> logger)
    {
        _personRepository = personRepository;
        _picturePersonService = picturePersonService;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(RemoveAvatarCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken: cancellationToken);
        if (person is null)
        {
            _logger.LogError("Person not found for Id: {Id}", request.PersonId);
            return ApiResponse.Error(ResponseCode.NotFound, "Person not found");
        }

        if (person.Picture is null)
        {
            _logger.LogError("Person does not have a picture");
            return ApiResponse.Ok();
        }

        await _picturePersonService.DeletePictureAsync(person.Picture);
            
        person.Picture = null;
            
        _personRepository.Update(person);
        await _personRepository.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok();
    }
}