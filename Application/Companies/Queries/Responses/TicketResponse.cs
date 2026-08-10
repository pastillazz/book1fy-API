using Domain.Enums;

namespace Application.Companies.Queries.Responses;

public record TicketResponse(
    Guid Id,
    Guid ServiceId,
    Guid UserId,
    TicketStatus Status,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc,
    decimal Price);
