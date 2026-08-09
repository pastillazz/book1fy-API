using Api.Abstractions;
using Api.Authentication;
using Application.Users.Commands;
using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Queries;
using Application.Users.Queries.GetUserByEmail;
using Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UsersController(ISender sender) : ApiController(sender)
{
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.FirstName,
            request.LastName, request.UserName,
            request.Email, request.Password,
            request.PhoneNumber);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure) return HandleFailure(result);

        return Ok(result.Value);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var command=new LoginUserCommand(
            request.Email,
            request.Password);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure) return HandleFailure(result);

        return Ok(result.Value);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        if (result.IsFailure) return HandleFailure(result);

        return Ok(result.Value);
    }

    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var query=new GetUserByEmailQuery(email);
        var result = await Sender.Send(query, cancellationToken);
        if (result.IsFailure) return HandleFailure(result);
        return Ok(result.Value);
    }

}
