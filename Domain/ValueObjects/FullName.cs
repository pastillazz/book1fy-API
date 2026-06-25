namespace Domain.ValueObjects;

public record FullName
{
    private string FirstName { get; init; }
    private string LastName { get; init; }
    
    public string Full => $"{FirstName} {LastName}";
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    
    public static FullName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("First name and last name cannot be empty.");

        if (firstName.Any(char.IsDigit) || lastName.Any(char.IsDigit))
        {
            throw new ArgumentException("First name and last name cannot contain numbers.");
        }
        
        return new FullName(firstName.Trim(), lastName.Trim());
    }
};