using Api.Abstractions;
using Api.Authentication;
using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Queries.GetUserByEmail;
using Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
public class UsersController(ISender sender) : ApiController(sender)
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.FirstName,
            request.LastName, request.UserName,
            request.Email, request.Password,
            request.PhoneNumber);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure) return ToProblemDetails(result.Error!);
        
        return Ok(result.Value);
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var command=new LoginUserCommand(
            request.Email,
            request.Password);
        
        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure) return ToProblemDetails(result.Error!);
        
        return Ok(result.Value);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        if (result.IsFailure) return ToProblemDetails(result.Error!);

        return Ok(result.Value);
    }
    
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var query=new GetUserByEmailQuery(email);
        var result = await Sender.Send(query, cancellationToken);
        if (result.IsFailure) return ToProblemDetails(result.Error!);
        return Ok(result.Value);
    }
    
}