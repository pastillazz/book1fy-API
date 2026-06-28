using Domain.Abstractions;
using Domain.Primitives;

namespace Domain.Entities;

public sealed class Service:AggregateRoot
{   
    private Service(Guid id, 
        string name,
        string description,
        bool isActive):base(id)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
    }
    
    protected Service()
    {
    }
    public string Name { get; private set; } 
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    
    public static Result<Service> Create(string name, 
        string description, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Error.None;
        }
        
        if (string.IsNullOrWhiteSpace(description))
        {
            return Error.None;
        }
        
        var service = new Service(Guid.NewGuid(), name, description, isActive);
        return service;
    }
}