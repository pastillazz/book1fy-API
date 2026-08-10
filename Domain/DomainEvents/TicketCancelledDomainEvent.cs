using Domain.Primitives;

namespace Domain.DomainEvents;

public record TicketCancelledDomainEvent(
    Guid Id,
    Guid TicketId,
    Guid CompanyId,
    string CompanyName,
    string CompanyEmail,
    Guid ServiceId,
    string ServiceName,
    Guid UserId,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc,
    decimal Price):IDomainEvent;
