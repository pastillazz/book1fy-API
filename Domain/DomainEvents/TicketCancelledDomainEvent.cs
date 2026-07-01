using Domain.Primitives;

namespace Domain.Entities;

public record TicketCancelledDomainEvent(Guid Id, Guid TicketId):DomainEvent(Id);