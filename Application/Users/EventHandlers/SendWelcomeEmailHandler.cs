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
        await emailService.SendWelcomeEmailAsync(
            notification.DomainEvent.Email, 
            notification.DomainEvent.Name, 
            cancellationToken);
    }
}