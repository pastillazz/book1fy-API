using System.Net;
using Domain.Shared;

namespace Domain.Errors;

public class UserErrors
{
    public static readonly Error UserAlreadyExists = new("User.AlreadyExists",
        "User with the same email already exists.",
        HttpStatusCode.Conflict);
    
    public static readonly Error UserNotFound = new("User.NotFound",
        "User with the specified ID or Email was not found.",
        HttpStatusCode.NotFound);
    
    public static readonly Error InvalidCredentials = new("User.InvalidCredentials",
        "Invalid password.", HttpStatusCode.Unauthorized);
}
