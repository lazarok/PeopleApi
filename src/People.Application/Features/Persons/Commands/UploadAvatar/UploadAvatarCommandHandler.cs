using MediatR;
using People.Application.Models;
using People.Application.Repositories.Common;
using People.Application.Services;
using People.Application.Services.Persons;

namespace People.Application.Features.Persons.Commands.UploadAvatar;

public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, ApiResponse>
{
    private readonly IRepository<Domain.Entities.Person> _personRepository;
    private readonly IPicturePersonService _picturePersonService;

    public UploadAvatarCommandHandler(IRepository<Domain.Entities.Person> personRepository, IPicturePersonService picturePersonService)
    {
        _personRepository = personRepository;
        _picturePersonService = picturePersonService;
   
    }
    
    public async Task<ApiResponse> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken: cancellationToken);
        if(person is null)
                return ApiResponse.Error(ResponseCode.NotFound, "Person not found");

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