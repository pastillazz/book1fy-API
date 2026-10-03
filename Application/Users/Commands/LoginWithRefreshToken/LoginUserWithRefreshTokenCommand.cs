using Application.Common.Abstractions.Interfaces;

namespace Application.Users.Commands.LoginWithRefreshToken;

public record LoginUserWithRefreshTokenCommand(string Token):ICommand<AuthResult>;

