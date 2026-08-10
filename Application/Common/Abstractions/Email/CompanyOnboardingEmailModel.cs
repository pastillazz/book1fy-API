namespace Application.Common.Abstractions.Email;


public sealed record CompanyOnboardingEmailModel(
    string RecipientEmail,
    string OwnerName,
    string CompanyName);
