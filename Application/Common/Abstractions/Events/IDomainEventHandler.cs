using Domain.Abstractions;
using Domain.Primitives;
using MediatR;

namespace Application.Common.Abstractions.Events;

public interface IDomainEventHandler<TEvent>:
    INotificationHandler<DomainEventNotification<TEvent>>
    where TEvent : IDomainEvent
{
    
}