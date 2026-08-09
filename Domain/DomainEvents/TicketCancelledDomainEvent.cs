using Domain.Primitives;

namespace Domain.DomainEvents;

public record TicketCancelledDomainEvent(
    Guid Id, Guid TicketId):IDomainEvent;