namespace Application.Common.Abstractions.Email;


public sealed record BookingEmailModel(
    string RecipientEmail,
    string RecipientName,
    string CustomerName,
    string CompanyName,
    string ServiceName,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc,
    decimal Price);
