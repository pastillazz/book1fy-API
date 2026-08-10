using Domain.Primitives;

namespace Domain.DomainEvents;

public record CompanyCreatedDomainEvent(
    Guid Id,
    Guid CompanyId,
    string CompanyName,
    string CompanyEmail,
    Guid OwnerId):IDomainEvent;
