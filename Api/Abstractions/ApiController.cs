using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Abstractions;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class ApiController:ControllerBase
{
    protected readonly ISender Sender;
    protected ApiController(ISender sender)=>Sender = sender;

    protected IActionResult HandleFailure(Result result) =>
        result switch
        { 
            {IsSuccess:true}=>throw new InvalidOperationException
                ("Cannot handle a successful result"),
            
            IValidationResult validationResult =>
               BadRequest(CreateProblemDetails(
                   "Validation Error",
                   StatusCodes.Status400BadRequest,
                   result.Error,
                   validationResult.Errors)),
            
            _ => StatusCode(
                (int)result.Error.StatusCode,
                CreateProblemDetails(
                    GetTitleForStatusCode((int)result.Error.StatusCode),
                    (int)result.Error.StatusCode,
                    result.Error))
        };
    
        private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null)
         {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = status,
            Type = error.Code,
            Detail = error.Message
        };
        
        if (errors is not null && errors.Length > 0)
        {
            problemDetails.Extensions["errors"] = errors;
        }
        return problemDetails;
         }
        private static string GetTitleForStatusCode(int statusCode) =>
            statusCode switch
            {
                 StatusCodes.Status400BadRequest => "Bad Request",
                 StatusCodes.Status401Unauthorized => "Unauthorized",
                 StatusCodes.Status403Forbidden => "Forbidden",
                 StatusCodes.Status404NotFound => "Not Found",
                 StatusCodes.Status409Conflict => "Conflict",
                 _ => "An error occurred"
            };
}
