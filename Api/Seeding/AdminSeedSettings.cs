using System.ComponentModel.DataAnnotations;

namespace Api.Seeding;

public class AdminSeedSettings
{
    public const string SectionName = "AdminSeed";
    
    [Required]
    public bool Enabled { get; init; }
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = 
        "First name must be between 2 and 50 characters")]
    public string FirstName { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = 
        "Last name must be between 2 and 50 characters")]
    public string LastName { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Username is required")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = 
        "Username must be between 3 and 30 characters")]
    public string UserName { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]    
    public string Password { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string PhoneNumber { get; init; } = string.Empty;
}
