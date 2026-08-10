using System.Text.Json;
using Application.Common.Abstractions.Events;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob
    (AppDbContext dbContext,
    IPublisher publisher,
    ILogger<ProcessOutboxMessagesJob> logger) : IJob
{
    private const int BatchSize = 20;
    private const int MaxRetries = 3;

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(BatchSize)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in messages)
        {
            try
            {
                await PublishAsync(outboxMessage, context.CancellationToken);

                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                outboxMessage.Error = null;
            }
            catch (Exception ex)
            {
                outboxMessage.RetryCount++;
                outboxMessage.Error = ex.ToString();

                if (outboxMessage.RetryCount >= MaxRetries)
                {
                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;

                    logger.LogError(ex,
                        "Outbox message {MessageId} of type {MessageType} " +
                        "gave up after {RetryCount} attempts.",
                        outboxMessage.Id, outboxMessage.Type,
                        outboxMessage.RetryCount);
                }
                else
                {
                    logger.LogWarning(ex,
                        "Outbox message {MessageId} of type {MessageType} " +
                        "failed on attempt {RetryCount}; will retry.",
                        outboxMessage.Id, outboxMessage.Type,
                        outboxMessage.RetryCount);
                }
            }
            
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }

    private async Task PublishAsync(
        OutboxMessage outboxMessage,
        CancellationToken cancellationToken)
    {
        Type? domainEventType = Type.GetType(outboxMessage.Type);

        if (domainEventType is null)
        {
            throw new InvalidOperationException(
                $"Unknown domain event type: {outboxMessage.Type}");
        }

        var domainEvent = JsonSerializer
            .Deserialize(outboxMessage.Content, domainEventType);

        if (domainEvent is null)
        {
            throw new InvalidOperationException(
                $"Could not deserialize outbox message {outboxMessage.Id}.");
        }

        var notificationType = typeof(DomainEventNotification<>)
            .MakeGenericType(domainEventType);

        var notification = Activator
            .CreateInstance(notificationType, domainEvent);

        if (notification is not INotification mediatrNotification)
        {
            throw new InvalidOperationException(
                $"Could not build a notification for {outboxMessage.Type}.");
        }

        await publisher.Publish(mediatrNotification, cancellationToken);
    }
}
