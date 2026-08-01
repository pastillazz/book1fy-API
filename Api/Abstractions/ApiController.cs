using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Abstractions;

[ApiController]
public class ApiController:ControllerBase
{   
    protected readonly ISender Sender;
    protected ApiController(ISender sender)=>Sender = sender;
}