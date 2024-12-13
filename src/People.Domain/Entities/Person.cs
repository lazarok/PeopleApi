using People.Domain.Common;

namespace People.Domain.Entities;

public class Person : BaseEntity
{
    public required string Fullname { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Picture { get; set; }
    public string? Dni { get; set; }
    public required DateTime CreatedAt { get; set; }
}