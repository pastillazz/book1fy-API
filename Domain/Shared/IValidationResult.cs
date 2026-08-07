using Domain.Abstractions;

namespace Domain.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = new (
        "Validation Error",
        "A validation problem occurred."
        );
   
    Error[] Errors { get; }
}