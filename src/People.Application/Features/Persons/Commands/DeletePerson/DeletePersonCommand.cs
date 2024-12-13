using MediatR;
using People.Application.Models;

namespace People.Application.Features.Persons.Commands.DeletePerson;

public record DeletePersonCommand(long Id) : IRequest<ApiResponse>;