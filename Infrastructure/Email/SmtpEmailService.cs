using Application.Common.Abstractions.Email;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Infrastructure.Messaging;

public class SmtpEmailService(IOptions<SmtpSettings> settings) : IEmailService
{
    private readonly SmtpSettings _settings = settings.Value;

    public async Task SendWelcomeEmailAsync(string email, string name,
        CancellationToken cancellationToken = default)
    {   
        var templatePath = Path.Combine(AppContext.BaseDirectory,
            "Email", "WelcomeEmail.html");

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException(
                $"Email template not found at: {templatePath}");
        }

        var htmlContent = await File.ReadAllTextAsync(templatePath,
            cancellationToken);
        
        htmlContent = htmlContent
            .Replace("{{Name}}", name)
            .Replace("{{Email}}", email);
        
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress(
            _settings.SenderName, 
            _settings.SenderEmail));
        message.To.Add(new MailboxAddress(name, email));
            
        message.Subject = "Welcome to Book1fy! 🗓️ Your account is ready";
        
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlContent
        };
        
        message.Body = bodyBuilder.ToMessageBody();
        
        var client = new SmtpClient();

        await client.ConnectAsync(
            _settings.Host, 
            _settings.Port, 
            SecureSocketOptions.StartTls, 
            cancellationToken);
        
        await client.AuthenticateAsync(
            _settings.SenderEmail, 
            _settings.Password, 
            cancellationToken
        );
        
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

}