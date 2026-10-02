using System;
using Application.Common.Abstractions.Authentication;
using Application.Common.Abstractions.Interfaces;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.LoginWithRefreshToken;

public class LoginUserWithRefreshTokenCommandHandler : ICommandHandler<LoginUserWithRefreshTokenCommand, AuthResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public LoginUserWithRefreshTokenCommandHandler(IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;

    }
    public async Task<Result<AuthResult>> Handle(LoginUserWithRefreshTokenCommand request, CancellationToken cancellationToken)
    {

        var refreshToken = await _userRepository.GetRefreshTokenAsync(request.Token, cancellationToken);

        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            return UserErrors.RefreshTokenNotFound;
        }

        var user = refreshToken.User;

        var accessToken = _jwtTokenGenerator.Generate(user);

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = _jwtTokenGenerator.GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        _userRepository.Add(newRefreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new AuthResult(user.Id,
        user.Username,
        user.Email.Value,
        accessToken,
        newRefreshToken.Token);
    }
}

