using Domain.Primitives;

namespace Domain.Entities;

public record TicketSoldDomainEvent(Guid Id, Guid TicketId):DomainEvent(Id);
