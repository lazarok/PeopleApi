namespace People.Application.Features.Persons.Dtos;

public class PersonDetailsDto
{
    public required long Id { get; set; }
    public required string Fullname { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PictureUrl { get; set; }
    public string? Dni { get; set; }
}