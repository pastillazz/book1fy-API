using Application.Abstractions.Interfaces;

namespace Application.Users.Commands;

public record CreateUserCommand(
    string FirstName,
    string LastName, 
    string Username, 
    string Email, 
    string Password,
    string PhoneNumber):ICommand;