using Domain.Abstractions;

namespace Domain.Errors;

public class UserErrors
{
    public static readonly Error UserAlreadyExists = new("User.AlreadyExists",
        "User with the same email already exists.");
}