using Domain.Primitives;

namespace Domain.DomainEvents;

public record TicketCreatedDomainEvent(Guid Id, Guid TicketId):DomainEvent(Id);