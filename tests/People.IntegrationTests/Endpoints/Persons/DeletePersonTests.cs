using System.Net.Http.Json;
using FluentAssert;
using Microsoft.EntityFrameworkCore;
using People.Application.Models;
using People.Infrastructure.Persistence.Context;
using People.IntegrationTests.Infrastructure;

namespace People.IntegrationTests.Endpoints.Persons;

[Collection(MySqlContainerFixture.FixtureName)]
public class DeletePersonTests : IClassFixture<WebApplicationFixture>
{
    private readonly HttpClient _client;
    private readonly PeopleContext _context;

    public DeletePersonTests(WebApplicationFixture factory)
    {
        _client = factory.HttpClient;
        _context = factory.DbContext;
    }

    [Fact]
    public async Task Should_ThrowError_When_Id_Not_Exists()
    {
        // Arrange
        var person = await _context.Persons.FirstAsync();
        var personId = person.Id;
        
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        
        var response = await _client.DeleteAsync($"/api/persons/{personId}");

        // Act
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Assert
        response.IsSuccessStatusCode.ShouldBeFalse();
        apiResponse.ShouldNotBeNull();
        apiResponse!.Success.ShouldBeFalse();
        apiResponse.Code.ShouldBeEqualTo(ResponseCode.NotFound);
    }
    
    [Fact]
    public async Task Should_DeletePerson_When_Id_Exists()
    {
        // Arrange
        var person = await _context.Persons.FirstAsync();
        
        var response = await _client.DeleteAsync($"/api/persons/{person.Id}");

        // Act
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Assert
        response.IsSuccessStatusCode.ShouldBeTrue();
        apiResponse.ShouldNotBeNull();
        apiResponse!.Success.ShouldBeTrue();
        
        (await _context.Persons.AnyAsync(x => x.Id == person.Id)).ShouldBeFalse();
    }
}