using Domain.Shared;

namespace Domain.Errors;

public class EmailErrors
{
    public static readonly Error Empty = new("Email.Empty",
        "Email cannot be empty.");
    
    public static readonly Error Invalid = new("Email.Invalid", 
        "Email format is invalid.");
}
