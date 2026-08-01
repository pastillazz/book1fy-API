using Application.Abstractions.Authentication;
using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;

namespace Application.Users.Commands.Register;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtGenerator jwtGenerator):ICommandHandler<CreateUserCommand,AuthResult>
{
    public async Task<Result<AuthResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser= await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null!) return UserErrors.UserAlreadyExists;
        
        var userCreated= User.Create(request.FirstName,request.LastName,
            request.UserName,request.Email, 
            request.Password, request.PhoneNumber, passwordHasher);
        
        if (userCreated.IsFailure) return userCreated.Error!;
        
        var user = userCreated.Value;
        
        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var token = jwtGenerator.Generate(user);
        
        return new AuthResult(
            user.Id,
            user.Username,
            user.Email.Value, 
            token);
    }
}

    
    
    
