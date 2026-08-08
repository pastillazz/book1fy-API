using Domain.Primitives;

namespace Domain.DomainEvents;

public record UserCreatedDomainEvent(Guid Id, Guid UserId):DomainEvent(Id);