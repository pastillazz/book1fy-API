using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Authentication;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    [Required(ErrorMessage = "Secret is required.")]
    [MinLength(32, ErrorMessage = "Secret must be at least 32 characters.")]
    public string Secret { get; init; } = string.Empty;
    [Required(ErrorMessage = "Issuer is required.")]
    public string Issuer { get; init; } = string.Empty;
    [Required(ErrorMessage = "Audience is required.")]
    public string Audience { get; init; } = string.Empty;
    [Required]
    [Range(1,1440, ErrorMessage = "ExpiryMinutes must be between 1 and 1440.")]
    public int ExpiryMinutes { get; init; }
}