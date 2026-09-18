using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Notifications;

public class SmtpSettings
{
    public const string SectionName = "SmtpSettings";
    
    [Required(ErrorMessage = "Host is required.")]
    public string Host { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Port is required.")]
    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535.")]
    public int Port { get; init; }
    
    [Required(ErrorMessage = "SenderName is required.")]
    [MaxLength(100, ErrorMessage = "SenderName cannot exceed 100 characters.")]
    public string SenderName { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "SenderEmail is required.")]
    [EmailAddress(ErrorMessage = "SenderEmail must be a valid email address.")]
    [MaxLength(255, ErrorMessage = "SenderEmail cannot exceed 255 characters.")]
    public string SenderEmail { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; init; } = string.Empty;
}