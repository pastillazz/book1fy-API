namespace Api.Authentication;

public record LoginRequest(
    string Email,
    string Password);