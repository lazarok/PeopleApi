using MediatR;
using People.Application.Models;
using People.Application.Services.Persons;

namespace People.Application.Features.Persons.Commands.UploadAvatar;

public record UploadAvatarCommand(long PersonId, MediaFileModel MediaFile) : IRequest<ApiResponse>;