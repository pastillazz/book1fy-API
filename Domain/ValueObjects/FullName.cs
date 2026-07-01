using Domain.Abstractions;
using Domain.ValueObjects.Errors;

namespace Domain.ValueObjects;

public sealed record FullName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public string Full => $"{FirstName} {LastName}";
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    
    public static Result<FullName> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            return FullNameErrors.Empty;

        if (firstName.Any(char.IsDigit) || lastName.Any(char.IsDigit))
        {
            return FullNameErrors.Invalid;
        }
        
        return new FullName(firstName, lastName);
    }
}