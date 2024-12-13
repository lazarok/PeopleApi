using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using People.Application.Models;

namespace People.Api.Controllers.Base;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
    
    protected ObjectResult BuildResponse(ApiResponse response)
    {
        // 2xx
        
        if (response.Success)
        {
            return StatusCode((int)HttpStatusCode.OK, response);
        }
        
        if (response.Code == ResponseCode.Created)
        {
            return StatusCode((int)HttpStatusCode.Created, response);
        }
        
        // Bad
        
        if (response.Code == ResponseCode.NotFound)
        {
            return StatusCode((int)HttpStatusCode.NotFound, response);
        }
        
        if (response.Code == ResponseCode.Unhandled)
        {
            return StatusCode((int)HttpStatusCode.Unauthorized, response);
        }
        
        if (response.Code == ResponseCode.Forbidden)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, response);
        }
        
        return StatusCode((int)HttpStatusCode.BadRequest, response);
    }
}