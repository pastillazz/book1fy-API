using Domain.Abstractions;

namespace Domain.ValueObjects.Errors;

public static class FullNameErrors
{
    public static readonly Error Empty = new("FullName.Empty",
        "First name and last name cannot be empty.");
    
    public static readonly Error Invalid = new("FullName.Invalid", 
        "First name and last name cannot contain numbers.");
}