using Domain.Enums;

namespace Application.Companies.Queries;

public record TicketResponse(
    Guid Id,
    Guid ServiceId,
    Guid UserId,
    string Status,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc,
    decimal Price);
