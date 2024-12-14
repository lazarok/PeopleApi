using MediatR;
using Microsoft.Extensions.Logging;
using People.Application.Models;
using People.Application.Repositories.Common;
using People.Application.Services.Persons;

namespace People.Application.Features.Persons.Commands.UploadAvatar;

public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, ApiResponse>
{
    private readonly IRepository<Domain.Entities.Person> _personRepository;
    private readonly IPicturePersonService _picturePersonService;
    private readonly ILogger<UploadAvatarCommandHandler> _logger;

    public UploadAvatarCommandHandler(
        IRepository<Domain.Entities.Person> personRepository,
        IPicturePersonService picturePersonService, 
        ILogger<UploadAvatarCommandHandler> logger)
    {
        _personRepository = personRepository;
        _picturePersonService = picturePersonService;
        _logger = logger;
    }
    
    public async Task<ApiResponse> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken: cancellationToken);
        if (person is null)
        {
            _logger.LogError("Person not found for Id: {Id}", request.PersonId);
            return ApiResponse.Error(ResponseCode.NotFound, "Person not found");
        }

        var filename = await _picturePersonService.UploadAsync(person, request.MediaFile.FileContent);
        
        // First time
        if (person.Picture != filename)
        {
            person.Picture = filename;
            _personRepository.Update(person);
            await _personRepository.SaveChangesAsync(cancellationToken);
        }

        return ApiResponse.Ok();
    }
}