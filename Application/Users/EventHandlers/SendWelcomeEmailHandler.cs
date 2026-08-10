using Application.Common.Abstractions.Email;
using Application.Common.Abstractions.Events;
using Domain.DomainEvents;

namespace Application.Users.EventHandlers;

public class SendWelcomeEmailHandler(IEmailService emailService):
    IDomainEventHandler<UserCreatedDomainEvent>
{
    public async Task Handle
        (DomainEventNotification<UserCreatedDomainEvent> notification,
            CancellationToken cancellationToken)
    {   
        var domainEvent = notification.DomainEvent;
        
        await emailService.SendWelcomeEmailAsync(
            domainEvent.Email, 
            domainEvent.Name, 
            cancellationToken);
    }
}