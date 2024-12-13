using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using People.Api.Controllers.Base;
using People.Application.Features.Persons.Commands.CreatePerson;
using People.Application.Features.Persons.Commands.DeletePerson;
using People.Application.Features.Persons.Commands.UpdatePerson;
using People.Application.Features.Persons.Dtos;
using People.Application.Features.Persons.Queries.GetPersonDetails;
using People.Application.Features.Persons.Queries.GetPersonList;
using People.Application.Models;

namespace People.Api.Controllers;

[Route("api/persons")]
public class PersonController(IMapper mapper) : BaseApiController
{
    /// <summary>
    /// Get person by id
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse<PersonDetailsDto>), StatusCodes.Status200OK)]
    [HttpGet("{personId}")]
    public async Task<IActionResult> Get(long personId)
    {
        return BuildResponse(await Mediator.Send(new GetPersonDetailsQuery(personId)));
    }
    
    /// <summary>
    /// Get persons
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse<PagedList<PersonListDto>>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetPersonListQuery query)
    {
        return BuildResponse(await Mediator.Send(query));
    }
    
    /// <summary>
    /// Create person
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse<PersonDetailsDto>), StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> CreatePerson([FromBody] CreatePersonCommand request)
    {
        return BuildResponse(await Mediator.Send(request));
    }
    
    /// <summary>
    /// Update person
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse<PersonDetailsDto>), StatusCodes.Status200OK)]
    [HttpPut("{personId}")]
    public async Task<IActionResult> UpdateProduct(long personId, [FromBody] UpdatePersonRequest request)
    {
        var command = mapper.Map<UpdatePersonCommand>((request, personId));
        return BuildResponse(await Mediator.Send(command));
    }
    
    /// <summary>
    /// Delete person
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [HttpDelete("{personId}")]
    public async Task<IActionResult> DeleteProduct(long personId)
    {
        return BuildResponse(await Mediator.Send(new DeletePersonCommand(personId)));
    }
}