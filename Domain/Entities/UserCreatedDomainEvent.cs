using Domain.Primitives;

namespace Domain.Entities;

public record UserCreatedDomainEvent(Guid Id, Guid UserId):DomainEvent(Id);