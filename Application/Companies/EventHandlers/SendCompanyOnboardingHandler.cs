using Application.Common.Abstractions.Email;
using Application.Common.Abstractions.Events;
using Application.Users.Queries;
using Domain.DomainEvents;

namespace Application.Companies.EventHandlers;

public class SendCompanyOnboardingHandler(
    IEmailService emailService,
    IUserQueries userQueries):
    IDomainEventHandler<CompanyCreatedDomainEvent>
{
    public async Task Handle
        (DomainEventNotification<CompanyCreatedDomainEvent> notification,
            CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var owner = await userQueries
            .GetUserByIdAsync(domainEvent.OwnerId, cancellationToken);

        var model = new CompanyOnboardingEmailModel(
            RecipientEmail: domainEvent.CompanyEmail,
            OwnerName: owner?.FirstName ?? domainEvent.CompanyName,
            CompanyName: domainEvent.CompanyName);

        await emailService
            .SendCompanyOnboardingAsync(model, cancellationToken);
    }
}
