using Application.Common.Abstractions.Authentication;
using Application.Common.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.Login;

public class LoginUserCommandHandler(IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator):
    ICommandHandler<LoginUserCommand,AuthResult>
{
    public async Task<Result<AuthResult>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    { 
        var email = request.Email.Trim().ToLowerInvariant();
        var user=await userRepository
           .GetByEmailAsync(email, cancellationToken);

       if (user is null) return UserErrors.InvalidCredentials;
       
       if (!user.HasPassword(request.Password, passwordHasher))
           return UserErrors.InvalidCredentials;
       
       var token = jwtTokenGenerator.Generate(user);
       
       return new AuthResult(
           user.Id, user.Username,
           user.Email.Value, token);
    }
}