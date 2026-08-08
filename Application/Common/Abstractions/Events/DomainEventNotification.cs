using Domain.Abstractions;
using MediatR;

namespace Application.Common.Abstractions.Events;

public class DomainEventNotification<TDomainEvent>
    (TDomainEvent domainEvent) : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; } = domainEvent;
}