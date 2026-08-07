using Application.Abstractions.Authentication;
using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.Register;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator):ICommandHandler<RegisterUserCommand,AuthResult>
{
    public async Task<Result<AuthResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser= await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (existingUser is not null) return UserErrors.UserAlreadyExists;
        
        var userCreated= User.Create(request.FirstName,request.LastName,
            request.UserName,request.Email, 
            request.Password, request.PhoneNumber, passwordHasher);
        
        if (userCreated.IsFailure) return userCreated.Error!;
        
        var user = userCreated.Value;
        
        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var token = jwtTokenGenerator.Generate(user);
        
        return new AuthResult(
            user.Id,
            user.Username,
            user.Email.Value, 
            token);
    }
}

    
    
    
