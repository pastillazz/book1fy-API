using Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Abstractions;

[ApiController]
[Authorize]
public class ApiController:ControllerBase
{   
    protected readonly ISender Sender;
    protected ApiController(ISender sender)=>Sender = sender;

    protected IActionResult ToProblemDetails(Error error)
    {
        var problemDetails = new ProblemDetails
        {
            Status = (int)error.StatusCode,
            Title = error.Code,
            Detail = error.Message
        };
        return StatusCode((int)error.StatusCode, problemDetails);
    }
}