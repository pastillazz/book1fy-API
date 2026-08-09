using System.Text.Json;
using Application.Common.Abstractions.Events;
using Domain.Abstractions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob
    (AppDbContext dbContext, IPublisher publisher) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null) 
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in messages)
        {
            try
            {
                Type? domainEventType = Type.GetType(outboxMessage.Type);

                if (domainEventType is null) continue;

                var domainEvent = JsonSerializer
                    .Deserialize(outboxMessage.Content,
                        domainEventType);

                if (domainEvent is null)
                {
                    continue;
                }

                var notificationType = typeof(DomainEventNotification<>)
                    .MakeGenericType(domainEventType);

                var notification = Activator
                    .CreateInstance(notificationType, domainEvent);

                if (notification is INotification mediatrNotification)
                {
                    await publisher.Publish(mediatrNotification,
                        context.CancellationToken);
                }
            }

            catch (Exception ex)
            {
                outboxMessage.Error = ex.ToString();
            }
            finally
            {
                outboxMessage.ProcessedOnUtc = DateTime.UtcNow; 
            }
        }
        
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}