namespace People.Application.Features.Persons.Dtos;

public class PersonListDto
{
    public required long Id { get; set; }
    public required string Fullname { get; set; }
    public required string Email { get; set; }
    public string? Dni { get; set; }
}