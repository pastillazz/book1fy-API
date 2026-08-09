using Domain.Abstractions;

namespace Domain.Primitives;

public abstract class AggregateRoot :Entity
{  
    private readonly List<IDomainEvent> _domainEvents=new();
    protected AggregateRoot(Guid id) : base(id)
    {
    }
    protected AggregateRoot()
    {
    }
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => 
        _domainEvents.ToList();
    
    public void ClearDomainEvents() => 
        _domainEvents.Clear();
    
    protected void RaiseDomainEvent(IDomainEvent domainEvent)=> 
        _domainEvents.Add(domainEvent);
    
}