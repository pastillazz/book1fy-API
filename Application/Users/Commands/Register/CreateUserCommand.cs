using Application.Abstractions.Interfaces;

namespace Application.Users.Commands;

public record CreateUserCommand(
    string FirstName,
    string LastName, 
    string UserName, 
    string Email, 
    string Password,
    string PhoneNumber):ICommand<AuthResult>
{
   
}