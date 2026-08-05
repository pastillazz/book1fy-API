using Application.Abstractions.Interfaces;

namespace Application.Users.Commands.Login;

public record LoginUserCommand(
    string Email,
    string Password):ICommand<AuthResult>;