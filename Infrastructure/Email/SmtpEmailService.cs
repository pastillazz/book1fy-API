using System.Globalization;
using System.Net;
using Application.Common.Abstractions.Email;
using Infrastructure.Messaging;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Infrastructure.Email;

public class SmtpEmailService(IOptions<SmtpSettings> settings) : IEmailService
{
    private readonly SmtpSettings _settings = settings.Value;

    public Task SendWelcomeEmailAsync(string email, string name,
        CancellationToken cancellationToken = default) =>
        SendAsync(
            template: "WelcomeEmail.html",
            subject: "Welcome to Book1fy! 🗓️ Your account is ready",
            toName: name,
            toEmail: email,
            placeholders: new Dictionary<string, string>
            {
                ["Name"] = name,
                ["Email"] = email
            },
            cancellationToken);

    public Task SendBookingConfirmationAsync(BookingEmailModel model,
        CancellationToken cancellationToken = default) =>
        SendAsync(
            template: "BookingConfirmationEmail.html",
            subject: $"Booking confirmed — {model.ServiceName} at {model.CompanyName}",
            toName: model.RecipientName,
            toEmail: model.RecipientEmail,
            placeholders: BookingPlaceholders(model),
            cancellationToken);

    public Task SendBookingCancellationAsync(BookingEmailModel model,
        CancellationToken cancellationToken = default) =>
        SendAsync(
            template: "BookingCancellationEmail.html",
            subject: $"Booking cancelled — {model.ServiceName} at {model.CompanyName}",
            toName: model.RecipientName,
            toEmail: model.RecipientEmail,
            placeholders: BookingPlaceholders(model),
            cancellationToken);

    public Task SendOwnerCancellationNoticeAsync(BookingEmailModel model,
        CancellationToken cancellationToken = default) =>
        SendAsync(
            template: "OwnerCancellationNoticeEmail.html",
            subject: $"Cancellation — {model.ServiceName} on {FormatDate(model.StartTimeUtc)}",
            toName: model.RecipientName,
            toEmail: model.RecipientEmail,
            placeholders: BookingPlaceholders(model),
            cancellationToken);

    public Task SendCompanyOnboardingAsync(CompanyOnboardingEmailModel model,
        CancellationToken cancellationToken = default) =>
        SendAsync(
            template: "CompanyOnboardingEmail.html",
            subject: $"{model.CompanyName} is live on Book1fy 🚀",
            toName: model.OwnerName,
            toEmail: model.RecipientEmail,
            placeholders: new Dictionary<string, string>
            {
                ["OwnerName"] = model.OwnerName,
                ["CompanyName"] = model.CompanyName,
                ["Email"] = model.RecipientEmail
            },
            cancellationToken);

    private static Dictionary<string, string> BookingPlaceholders(
        BookingEmailModel model) =>
        new()
        {
            ["Name"] = model.RecipientName,
            ["Email"] = model.RecipientEmail,
            ["CustomerName"] = model.CustomerName,
            ["CompanyName"] = model.CompanyName,
            ["ServiceName"] = model.ServiceName,
            ["Date"] = FormatDate(model.StartTimeUtc),
            ["StartTime"] = FormatTime(model.StartTimeUtc),
            ["EndTime"] = FormatTime(model.EndTimeUtc),
            ["Duration"] = FormatDuration(model.EndTimeUtc - model.StartTimeUtc),
            ["Price"] = model.Price.ToString("N2", CultureInfo.InvariantCulture)
        };

    private static string FormatDate(DateTime utc) =>
        utc.ToString("dddd, d MMMM yyyy", CultureInfo.InvariantCulture);

    private static string FormatTime(DateTime utc) =>
        utc.ToString("HH:mm", CultureInfo.InvariantCulture);

    private static string FormatDuration(TimeSpan span) =>
        span.TotalMinutes < 60
            ? $"{(int)span.TotalMinutes} min"
            : span.Minutes == 0
                ? $"{(int)span.TotalHours} h"
                : $"{(int)span.TotalHours} h {span.Minutes} min";

    private async Task SendAsync(
        string template,
        string subject,
        string toName,
        string toEmail,
        IReadOnlyDictionary<string, string> placeholders,
        CancellationToken cancellationToken)
    {
        var htmlContent = await RenderAsync(
            template, placeholders, cancellationToken);

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            _settings.SenderName,
            _settings.SenderEmail));
        message.To.Add(new MailboxAddress(toName, toEmail));

        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlContent
        };

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();

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

    private static async Task<string> RenderAsync(
        string template,
        IReadOnlyDictionary<string, string> placeholders,
        CancellationToken cancellationToken)
    {
        var templatePath = Path.Combine(AppContext.BaseDirectory,
            "Email", template);

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException(
                $"Email template not found at: {templatePath}");
        }

        var htmlContent = await File
            .ReadAllTextAsync(templatePath,
            cancellationToken);

        foreach (var (key, value) in placeholders)
        {
           
            htmlContent = htmlContent.Replace(
                $"{{{{{key}}}}}", WebUtility.HtmlEncode(value));
        }

        return htmlContent;
    }
}
