using Domain.Abstractions;

namespace Domain.ValueObjects.Errors;

public class EmailErrors
{
    public static readonly Error Empty = new("Email.Empty",
        "Email cannot be empty.");
    
    public static readonly Error Invalid = new("Email.Invalid", 
        "Email format is invalid.");
}