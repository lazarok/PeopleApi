using MediatR;
using People.Application.Features.Persons.Dtos;
using People.Application.Models;

namespace People.Application.Features.Persons.Commands.UpdatePerson;

public class UpdatePersonCommand : IRequest<ApiResponse<PersonDetailsDto>>
{
    public required long Id { get; set; }
    public required string Fullname { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Dni { get; set; }
}

public class UpdatePersonRequest
{
    public required string Fullname { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Dni { get; set; }
    
    
}
