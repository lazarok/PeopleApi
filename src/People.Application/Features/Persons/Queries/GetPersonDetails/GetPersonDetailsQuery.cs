using MediatR;
using People.Application.Features.Persons.Dtos;
using People.Application.Models;

namespace People.Application.Features.Persons.Queries.GetPersonDetails;

public record GetPersonDetailsQuery(long Id) : IRequest<ApiResponse<PersonDetailsDto>> {}