namespace Application.Common.Abstractions.Email;

public interface IEmailService
{
    Task SendWelcomeEmailAsync
        (string email, string name, CancellationToken cancellationToken = default);
    Task SendBookingConfirmationAsync
        (BookingEmailModel model, CancellationToken cancellationToken = default);
    Task SendBookingCancellationAsync
        (BookingEmailModel model, CancellationToken cancellationToken = default);
    Task SendOwnerCancellationNoticeAsync
        (BookingEmailModel model, CancellationToken cancellationToken = default);
    Task SendCompanyOnboardingAsync
        (CompanyOnboardingEmailModel model, CancellationToken cancellationToken = default);
}
