namespace Application.Users.Queries;

public record UserResponse(
    Guid Id, 
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string PhoneNumber);