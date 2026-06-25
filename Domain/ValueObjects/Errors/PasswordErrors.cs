using Domain.Abstractions;

namespace Domain.ValueObjects.Errors;

public class PasswordErrors
{
    public static readonly Error Empty = new("Password.Empty",
        "Password cannot be empty.");
    
    public static readonly Error Weak = new("Password.Weak", 
        "Password must be at least 8 characters " +
        "long and contain at least one uppercase letter," +
        " one lowercase letter, one digit, and one special character.");
}