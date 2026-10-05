namespace Application.Common.Google;

public record GoogleUserInfo(
    string Subject,
    string Email,
    string? FamilyName,
    string? Name);