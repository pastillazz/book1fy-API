using Application.Common.Abstractions.Authentication;
using Application.Common.Abstractions.Interfaces;
using Domain.Repositories;
using Domain.Shared;


namespace Application.Users.Commands.RevokeRefreshTokens;

public class RevokeRefreshTokensCommandHandler:ICommandHandler<RevokeRefreshTokensCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public RevokeRefreshTokensCommandHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userContext = userContext;
        _userRepository = userRepository;
    }
    
    public async Task<Result> Handle(RevokeRefreshTokensCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        var isDeleted= await _userRepository.DeleteRefreshTokensByUserIdAsync(userId, cancellationToken);

        if (isDeleted)
        {
            return Result.Success();
        }

        throw new Exception("Revoke Refresh Tokens command failed");
    }
}