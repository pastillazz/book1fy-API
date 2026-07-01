using Domain.Abstractions;
using Domain.Enums;
using Domain.Primitives;
using Domain.ValueObjects.Errors;

namespace Domain.Entities;

public class Company:AggregateRoot
{   
    private readonly List<Service> _services=new();
    private Company(Guid id, string name, string description ) : base(id)
    {
        Name = name;
        Description = description;
        Status = CompanyStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }
    protected Company()
    {}
    public string Name { get; private set; }
    public string Description { get; private set; }
    public CompanyStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<Service> Services => _services;
    
    public static Company Create(Guid id, string name, string description)
    {   
        return new Company(id, name, description);
    }
    
    public Service AddService(Guid id, string name, 
        string description, TimeSpan openingTime,
        TimeSpan closingTime, List<DayOfWeek> workDays,decimal price)
    {
        var service= Service.Create(id, this.Id, name, description, 
            openingTime, closingTime, workDays, price);
        
        _services.Add(service);
        return service;
    }

    public Result<Ticket> AddTicketToService(Guid serviceId, Guid userId, DateTime startTimeUtc,
        DateTime endTimeUtc)
    {   
        var service=_services.FirstOrDefault(s => s.Id == serviceId);
        
        if (service == null)
        {
            return Result<Ticket>.Failure(ServiceErrors.NotFound);
        }
        
        var ticket = service.AddTicketToService(serviceId,userId,startTimeUtc, endTimeUtc);
        var TicketEvent= new TicketCreatedDomainEvent(Guid.NewGuid(), ticket.Id);
        RaiseDomainEvent(TicketEvent);
        return ticket;
    }

    public Result CancelTicket(Guid serviceId, Guid ticketId)
    {
        var service = _services.FirstOrDefault(s => s.Id == serviceId);
        if (service == null)
        {
            return Result.Failure(ServiceErrors.NotFound);
        }
        var result = service.CancelTicket(ticketId);
        if (result.IsFailure)
        {
            return result.Error!;
        }
        var TicketEvent= new TicketCancelledDomainEvent(Guid.NewGuid(), ticketId);
        RaiseDomainEvent(TicketEvent);
        return Result.Success();
    }
    public Result SellTicket(Guid serviceId, Guid ticketId)
    {
        var service = _services.FirstOrDefault(s => s.Id == serviceId);
        if (service == null)
        {
            return Result.Failure(ServiceErrors.NotFound);
        }
        var result = service.SellTicket(ticketId);
        
        if (result.IsFailure)
        {
            return result.Error!;
        }
        
        return Result.Success();
    }
}