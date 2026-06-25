using Domain.Abstractions;

namespace Domain.ValueObjects;

public record Password
{
    public string Hash { get; init; }
    
    private Password(string hash)=>Hash=hash;
    
    public static Password Create(string password, IPasswordHasher passwordHasher)
    {   
        ValidateStrength(password);
        var hash = passwordHasher.Hash(password);
        return new Password(hash);
    }

    public static Password FromDatabase(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            throw new ArgumentException("Hash cannot be null or empty.", nameof(hash));
        }
        
        return new Password(hash);
    }
    
    private static void ValidateStrength(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        if (password.Length < 8)
        {
            throw new ArgumentException("Password must be at least 8 characters long.");
        }

        if (!password.Any(char.IsUpper))
        {
            throw new ArgumentException("Password must contain at least one uppercase letter.");
        }
        
        if (!password.Any(char.IsDigit))
        {
            throw new ArgumentException("Password must contain at least one digit.");
        }

        if (password.All(char.IsLetterOrDigit))
        {
            throw new ArgumentException("Password must contain at least one special character.");
        }
    }
};