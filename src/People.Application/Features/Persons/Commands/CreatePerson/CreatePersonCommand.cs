using MediatR;
using People.Application.Features.Persons.Dtos;
using People.Application.Models;

namespace People.Application.Features.Persons.Commands.CreatePerson;

public record CreatePersonCommand : IRequest<ApiResponse<PersonDetailsDto>>
{
    public required string Fullname { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Dni { get; set; }
}