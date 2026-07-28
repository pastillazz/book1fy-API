using Domain.Abstractions;

namespace Domain.Errors;

public class UserErrors
{
    public static readonly Error UserAlreadyExists = new("User.AlreadyExists",
        "User with the same email already exists.");
    public static readonly Error UserNotFound = new("User.NotFound",
        "User with the specified ID or Email was not found.");
}