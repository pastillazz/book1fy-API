using Application.Common.Abstractions.Email;
using Application.Common.Abstractions.Events;
using Application.Users.Queries;
using Domain.DomainEvents;

namespace Application.Companies.EventHandlers;

public class NotifyOwnerOfCancellationHandler(
    IEmailService emailService,
    IUserQueries userQueries):
    IDomainEventHandler<TicketCancelledDomainEvent>
{
    public async Task Handle
        (DomainEventNotification<TicketCancelledDomainEvent> notification,
            CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var customer = await userQueries
            .GetUserByIdAsync(domainEvent.UserId, cancellationToken);
        
        var customerName = customer is null
            ? "A customer"
            : $"{customer.FirstName} {customer.LastName}";

        var model = new BookingEmailModel(
            RecipientEmail: domainEvent.CompanyEmail,
            RecipientName: domainEvent.CompanyName,
            CustomerName: customerName,
            CompanyName: domainEvent.CompanyName,
            ServiceName: domainEvent.ServiceName,
            StartTimeUtc: domainEvent.StartTimeUtc,
            EndTimeUtc: domainEvent.EndTimeUtc,
            Price: domainEvent.Price);

        await emailService
            .SendOwnerCancellationNoticeAsync(model, cancellationToken);
    }
}
