using Domain.Abstractions;

namespace Domain.Primitives;

public record DomainEvent(Guid Id):IDomainEvent
{
    public DateTime OccurredOn { get; }=DateTime.UtcNow;
}