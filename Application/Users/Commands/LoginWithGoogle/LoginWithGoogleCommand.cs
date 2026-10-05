using Application.Common.Abstractions.Interfaces;

namespace Application.Users.Commands.LoginWithGoogle;

public record LoginWithGoogleCommand(
    string IdToken
    ):ICommand<AuthResult>;