namespace Application.Common.Abstractions.Email;

public interface IEmailService
{
    Task SendWelcomeEmailAsync
        (string email, string name, CancellationToken cancellationToken = default);
}