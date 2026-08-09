namespace Infrastructure.Messaging;

public class SmtpSettings
{
    public const string SectionName = "SmtpSettings";

    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    public string SenderName { get; init; } = string.Empty;
    public string SenderEmail { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}