using Application.Common.Abstractions.Email;
using Application.Common.Abstractions.Events;
using Application.Users.Queries;
using Domain.DomainEvents;

namespace Application.Companies.EventHandlers;

public class SendBookingConfirmationHandler(
    IEmailService emailService,
    IUserQueries userQueries):
    IDomainEventHandler<TicketCreatedDomainEvent>
{
    public async Task Handle
        (DomainEventNotification<TicketCreatedDomainEvent> notification,
            CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        
        var customer = await userQueries
            .GetUserByIdAsync(domainEvent.UserId, cancellationToken);

        if (customer is null) return;

        var model = new BookingEmailModel(
            RecipientEmail: customer.Email,
            RecipientName: customer.FirstName,
            CustomerName: $"{customer.FirstName} {customer.LastName}",
            CompanyName: domainEvent.CompanyName,
            ServiceName: domainEvent.ServiceName,
            StartTimeUtc: domainEvent.StartTimeUtc,
            EndTimeUtc: domainEvent.EndTimeUtc,
            Price: domainEvent.Price);
        
        await emailService
            .SendBookingConfirmationAsync(model, cancellationToken);
    }
}
