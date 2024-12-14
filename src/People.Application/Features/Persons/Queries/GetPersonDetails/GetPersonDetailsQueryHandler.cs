using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using People.Application.Features.Persons.Dtos;
using People.Application.Models;
using People.Application.Repositories.Common;

namespace People.Application.Features.Persons.Queries.GetPersonDetails;

public class GetPersonDetailsQueryHandler : IRequestHandler<GetPersonDetailsQuery, ApiResponse<PersonDetailsDto>>
{
    private readonly IRepository<Domain.Entities.Person> _personRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GetPersonDetailsQueryHandler> _logger;

    public GetPersonDetailsQueryHandler(
        IRepository<Domain.Entities.Person> personRepository, 
        IConfiguration configuration, 
        ILogger<GetPersonDetailsQueryHandler> logger)
    {
        _personRepository = personRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ApiResponse<PersonDetailsDto>> Handle(GetPersonDetailsQuery request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (person is null)
        {
            _logger.LogError("Person not found for Id: {Id}", request.Id);
            return ApiResponse.Error<PersonDetailsDto>(ResponseCode.NotFound, "Person not found");
        }

        var response = ApiResponse.OkMapped<PersonDetailsDto>(person);

        if (person.Picture is not null)
        {
            response.Data!.PictureUrl = $"{_configuration["AppUrl"]}/api/mediafiles/preview/{person.Picture}";
        }
        
        return response;
    }
}