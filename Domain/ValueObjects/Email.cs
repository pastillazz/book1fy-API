using Domain.Abstractions;
using Domain.ValueObjects.Errors;

namespace Domain.ValueObjects;

public sealed record Email
{   
    private Email(string value) => Value = value;
    public string Value { get; private init; }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return EmailErrors.Empty;

        if (!value.Contains("@") || !value.Contains("."))
            return EmailErrors.Invalid;

        return new Email(value);
    }

};