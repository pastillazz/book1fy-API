using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed record Password
{
    public string Hash { get; init; }
    
    private Password(string hash)=>Hash=hash;
    
    public static Result<Password> Create(string password, IPasswordHasher passwordHasher)
    {
        if (ValidateStrength(password).IsFailure)
        {
            return PasswordErrors.Weak;
        }
        
        var hash = passwordHasher.Hash(password);
        return new Password(hash);
    }
    
    public bool Verify(string plainPassword,
        IPasswordHasher passwordHasher) =>
        passwordHasher.Verify(plainPassword, Hash);
 
    private static Result ValidateStrength(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return PasswordErrors.Empty;
        }

        if (password.Length < 8)
        {
            return PasswordErrors.Weak;
        }

        if (!password.Any(char.IsUpper))
        {
            return PasswordErrors.Weak;
        }
        
        if (!password.Any(char.IsDigit))
        {
            return PasswordErrors.Weak;
        }

        if(password.All(char.IsLetterOrDigit))
        {
            return PasswordErrors.Weak;
        }
        
        return Result.Success();
    }
};

