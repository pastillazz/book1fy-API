using Application.Common.Abstractions.Events;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions options, 
    IPublisher publisher):DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
     
        var result= await base.SaveChangesAsync(cancellationToken);
        
        await DispatchDomainEventsAsync(cancellationToken);
        
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker
            .Entries<AggregateRoot>() 
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();
        
        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();
        
        domainEntities.ForEach(x => x.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());
            
            var notification = Activator
                .CreateInstance(notificationType, domainEvent);
            
            if (notification is INotification mediatrNotification)
            {
                await publisher.Publish(mediatrNotification, 
                    cancellationToken);
            }
        }
    }
}

