using Domain.Primitives;

namespace Domain.Entities;

public record TicketCreatedDomainEvent(Guid Id, Guid TicketId):DomainEvent(Id);