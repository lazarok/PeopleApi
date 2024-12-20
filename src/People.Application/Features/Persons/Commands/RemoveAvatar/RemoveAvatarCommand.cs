using MediatR;
using People.Application.Models;

namespace People.Application.Features.Persons.Commands.RemoveAvatar;

public record RemoveAvatarCommand(long PersonId) : IRequest<ApiResponse>;