namespace Domain.ValueObjects;

public record Email
{   
    private Email(string value) => Value = value;
    public string Value { get; private init; }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.");

        if (!value.Contains("@") || !value.Contains("."))
            throw new ArgumentException("Invalid email format.");

        return new Email(value.ToLower().Trim());
    }

};