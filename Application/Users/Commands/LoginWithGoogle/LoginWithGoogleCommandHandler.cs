using Application.Common.Abstractions.Authentication;
using Application.Common.Abstractions.Interfaces;
using Application.Common.Google;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.LoginWithGoogle;

public class LoginWithGoogleCommandHandler : ICommandHandler<LoginWithGoogleCommand, AuthResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoogleTokenValidator _googleTokenValidator;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginWithGoogleCommandHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork, IGoogleTokenValidator googleTokenValidator,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _googleTokenValidator = googleTokenValidator;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthResult>> Handle(LoginWithGoogleCommand request, CancellationToken cancellationToken)
    {
       var googleUser = await _googleTokenValidator.ValidateAsync(request.IdToken);
       if (googleUser is null)
       {
           return UserErrors.UserNotFound;
       }
       
       var existingUser = await _userRepository.GetByEmailAsync(googleUser.Email, cancellationToken);
       
       if (existingUser is null)
       {
           var newUser = User.CreateWithGoogle(
               googleUser.Name,
               googleUser.FamilyName,
               googleUser.Email,
               googleUser.Subject);
           if (newUser.IsFailure) return newUser.Error;
        
           var user = newUser.Value;
        
           _userRepository.Add(user);
           
           var token = _jwtTokenGenerator.Generate(user);

           var refreshToken = new RefreshToken
           {
               Id = Guid.NewGuid(),
               UserId = user.Id,
               Token = _jwtTokenGenerator.GenerateRefreshToken(),
               ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
           };
        
           _userRepository.Add(refreshToken);
           await _unitOfWork.SaveChangesAsync(cancellationToken);
           return new AuthResult(
               user.Id,
               user.Username,
               user.Email.Value,
               token,
               refreshToken.Token);
       }
       
       var newToken = _jwtTokenGenerator.Generate(existingUser);

       var newRefreshToken = new RefreshToken
       {
           Id = Guid.NewGuid(),
           UserId = existingUser.Id,
           Token = _jwtTokenGenerator.GenerateRefreshToken(),
           ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
       };
       
       _userRepository.Add(newRefreshToken);
       await _unitOfWork.SaveChangesAsync(cancellationToken);
       
       return new AuthResult(
           existingUser.Id,
           existingUser.Username,
           existingUser.Email.Value,
           newToken,
           newRefreshToken.Token);
    }
}