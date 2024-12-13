using MediatR;
using People.Application.Features.Persons.Dtos;
using People.Application.Models;

namespace People.Application.Features.Persons.Queries.GetPersonList;

public class GetPersonListQuery : PaginationFilter, IRequest<ApiResponse<PagedList<PersonListDto>>> {}