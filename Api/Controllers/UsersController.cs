using Api.Abstractions;
using Application.Users.Commands;
using Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UsersController:ApiController
{ 
    protected UsersController(ISender sender) : base(sender) {}
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(request.FirstName,
            request.LastName, request.UserName,
            request.Email, request.Password,
            request.PhoneNumber);

        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
}