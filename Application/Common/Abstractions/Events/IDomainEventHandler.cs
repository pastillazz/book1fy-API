using Domain.Abstractions;
using MediatR;

namespace Application.Common.Abstractions.Events;

public interface IDomainEventHandler<TEvent>:
    INotificationHandler<DomainEventNotification<TEvent>>
    where TEvent : IDomainEvent
{
    
}