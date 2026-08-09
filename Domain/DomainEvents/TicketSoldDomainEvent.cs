using Domain.Primitives;

namespace Domain.DomainEvents;

public record TicketSoldDomainEvent(
    Guid Id, Guid TicketId):IDomainEvent;
