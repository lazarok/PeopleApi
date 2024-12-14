using System.Net.Http.Json;
using FluentAssert;
using People.Application.Features.Persons.Dtos;
using People.Application.Models;
using People.IntegrationTests.Infrastructure;

namespace People.IntegrationTests.Endpoints.Persons;

[Collection(MySqlContainerFixture.FixtureName)]
public class GetPersonsTests : IClassFixture<WebApplicationFixture>
{
    private readonly HttpClient _client;

    public GetPersonsTests(WebApplicationFixture factory)
    {
        _client = factory.HttpClient;
    }

    [Fact]
    public async Task Should_ReturnMultiplePersons()
    {
        // Arrange
        var response = await _client.GetAsync("/api/persons");

        // Act
        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<PagedList<PersonListDto>>>();

        // Assert
        apiResponse.ShouldNotBeNull();
        apiResponse!.Success.ShouldBeTrue();
        apiResponse!.Data.List.Count.ShouldBeGreaterThan(0);
    }
}