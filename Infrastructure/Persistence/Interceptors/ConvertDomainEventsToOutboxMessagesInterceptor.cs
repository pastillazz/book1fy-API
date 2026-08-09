using System.Text.Json;
using Domain.Primitives;
using Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
    :SaveChangesInterceptor
{
    
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;
        
        if (dbContext is null)
        {
           return base.SavingChangesAsync
               (eventData, result, cancellationToken);
        }
        
        var domainEntities = dbContext.ChangeTracker
            .Entries<AggregateRoot>() 
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();
        
        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();
        
        domainEntities.ForEach(x => x.ClearDomainEvents());
        
        var outboxMessages = domainEvents.Select(domainEvent => new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = domainEvent.GetType().AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(domainEvent, 
                domainEvent.GetType())
        }).ToList();
        
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}