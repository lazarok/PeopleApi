using System.Net.Http.Json;
using Bogus;
using Bogus.Extensions.UnitedStates;
using FluentAssert;
using Microsoft.EntityFrameworkCore;
using People.Application.Features.Persons.Commands.CreatePerson;
using People.Application.Features.Persons.Dtos;
using People.Application.Models;
using People.Infrastructure.Persistence.Context;
using People.IntegrationTests.Infrastructure;

namespace People.IntegrationTests.Endpoints.Persons;

[Collection(MySqlContainerFixture.FixtureName)]
public class CreatePersonTests : IClassFixture<WebApplicationFixture>
{
    private readonly HttpClient _client;
    private readonly PeopleContext _context;

    public CreatePersonTests(WebApplicationFixture factory)
    {
        _client = factory.HttpClient;
        _context = factory.DbContext;
    }

    [Fact]
    public async Task Should_CreatePerson_When_Data_Is_Valid()
    {
        // Arrange
        var command = new Faker<CreatePersonCommand>()
            .RuleFor(p => p.Fullname, f => f.Person.FullName)
            .RuleFor(p => p.DateOfBirth, f => f.Person.DateOfBirth)
            .RuleFor(p => p.Email, f => f.Person.Email.ToLower())
            .RuleFor(p => p.PhoneNumber, f => f.Random.Bool() ? f.Phone.PhoneNumber("+###########") : null)  // Optionl
            .RuleFor(p => p.Dni, f => f.Random.Bool() ? f.Person.Ssn() : null)  // Optionl
            .Generate();
        
        var response = await _client.PostAsJsonAsync("/api/persons", command);

        // Act
        response.EnsureSuccessStatusCode();
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<PersonDetailsDto>>();

        // Assert
        apiResponse.ShouldNotBeNull();
        apiResponse!.Success.ShouldBeTrue();
        
        apiResponse!.Data.Fullname.ShouldBeEqualTo(command.Fullname);
        apiResponse!.Data.DateOfBirth.ShouldBeEqualTo(command.DateOfBirth);
        apiResponse!.Data.Email.ShouldBeEqualTo(command.Email);
        apiResponse!.Data.PhoneNumber.ShouldBeEqualTo(command.PhoneNumber);
        apiResponse!.Data.Dni.ShouldBeEqualTo(command.Dni);
    }
    
    [Fact]
    public async Task Should_ThrowError_When_Email_Exists()
    {
        // Arrange
        var person = await _context.Persons.FirstAsync();
        
        var command = new Faker<CreatePersonCommand>()
            .RuleFor(p => p.Fullname, f => f.Person.FullName)
            .RuleFor(p => p.DateOfBirth, f => f.Person.DateOfBirth)
            .RuleFor(p => p.Email, f => person.Email)
            .RuleFor(p => p.PhoneNumber, f => f.Random.Bool() ? f.Phone.PhoneNumber("+###########") : null) // Optionl
            .RuleFor(p => p.Dni, f => f.Random.Bool() ? f.Person.Ssn() : null)  // Optionl
            .Generate();
        
        var response = await _client.PostAsJsonAsync("/api/persons", command);

        // Act
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<PersonDetailsDto>>();

        // Assert
        response.IsSuccessStatusCode.ShouldBeFalse();
        apiResponse.ShouldNotBeNull();
        apiResponse!.Success.ShouldBeFalse();
    }
}