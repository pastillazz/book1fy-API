using Application.Abstractions.Interfaces;
using Domain.Abstractions;
using Domain.Repositories;

namespace Application.Users.Commands;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork):ICommandHandler<CreateUserCommand>
{
    public Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

    
    
    
