using Application.Common.Abstractions.Interfaces;

namespace Application.Users.Commands.Register;

public record RegisterUserCommand(
    string FirstName,
    string LastName, 
    string UserName, 
    string Email, 
    string Password,
    string PhoneNumber):ICommand<AuthResult>
{
   
}