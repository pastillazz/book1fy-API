namespace Application.Users.Commands;

public record AuthResult(
    Guid Id,
    string Username, 
    string Email,
    string Token);