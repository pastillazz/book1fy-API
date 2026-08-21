namespace Api.Seeding;

public class AdminSeedSettings
{
    public const string SectionName = "AdminSeed";

    public bool Enabled { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
}
