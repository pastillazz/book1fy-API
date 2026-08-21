using Application.Common.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.SeedAdmin;

public class EnsureAdminUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher):ICommandHandler<EnsureAdminUserCommand,Guid>
{
    public async Task<Result<Guid>> Handle(EnsureAdminUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser= await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (existingUser is not null)
        {
            if (existingUser.IsAdmin) return existingUser.Id;

            var grantResult = existingUser.GrantAdminRole();
            if (grantResult.IsFailure) return grantResult.Error;

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return existingUser.Id;
        }

        var adminCreated= User.CreateAdmin(request.FirstName,request.LastName,
            request.UserName,request.Email,
            request.Password, request.PhoneNumber, passwordHasher);

        if (adminCreated.IsFailure) return adminCreated.Error;

        var admin = adminCreated.Value;

        userRepository.Add(admin);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return admin.Id;
    }
}
