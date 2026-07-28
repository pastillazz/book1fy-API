using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;

namespace Application.Users.Commands;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher):ICommandHandler<CreateUserCommand>
{
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser= await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null!) return UserErrors.UserAlreadyExists;
        
        var userCreated= User.Create(request.FirstName,request.LastName,
            request.Username,request.Email, 
            request.Password, request.PhoneNumber, passwordHasher);
        
        if (userCreated.IsFailure) return userCreated.Error!;
        
        userRepository.Add(userCreated.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

    
    
    
