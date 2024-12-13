using MediatR;
using People.Application.Extensions;
using People.Application.Features.Persons.Dtos;
using People.Application.Models;
using People.Application.Repositories.Common;

namespace People.Application.Features.Persons.Queries.GetPersonList;

public class GetPersonListQueryHandler : IRequestHandler<GetPersonListQuery, ApiResponse<PagedList<PersonListDto>>>
{
    private readonly IRepository<Domain.Entities.Person> _personRepository;

    public GetPersonListQueryHandler(IRepository<Domain.Entities.Person> personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<ApiResponse<PagedList<PersonListDto>>> Handle(GetPersonListQuery request, CancellationToken cancellationToken)
    {
        var search = request.Search?.Trim().ToLower();
        
        var queryable = _personRepository.GetAll();

        if (!string.IsNullOrEmpty(search))
            queryable = queryable.Where(x => x.Fullname.Contains(search) || x.Email.Contains(search));
        
        var pagedList = await queryable.ToPagedListAsync(request);
        
        return ApiResponse.OkMapped<PagedList<PersonListDto>>(pagedList);
    }
}