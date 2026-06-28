namespace Domain.Primitives;

public abstract class AggregateRoot :Entity
{  
    private readonly List<DomainEvent> _domainEvents=new();
    protected AggregateRoot(Guid id) : base(id)
    {
    }
    protected AggregateRoot()
    {
    }
    
    public IReadOnlyCollection<DomainEvent> DomainEvents => 
        _domainEvents.ToList();
    
    public void ClearDomainEvents() => 
        _domainEvents.Clear();
    
    protected void RaiseDomainEvent(DomainEvent domainEvent)=> 
        _domainEvents.Add(domainEvent);
    
}