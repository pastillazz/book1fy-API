namespace Api.Authentication;

public record AuthResponse(    
    Guid Id,
    string Username,
    string Email);