using Domain.Abstractions;
using Domain.Enums;
using Domain.Errors;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Company:AggregateRoot
{   
    private readonly List<Service> _services=new();
    private Company(Guid id, string name, string description,
        Email email ) : base(id)
    {
        Name = name;
        Description = description;
        Email = email;
        Status = CompanyStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }
    private Company()
    {
        Name = null!;
        Description = null!;
        Email = null!;
    }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Email Email { get; private set; }
    public CompanyStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<Service> Services => _services;
    
    public static Result<Company> Create( string name, 
        string description, string email)
    {   
        var emailResult = Email.Create(email);
        if (emailResult.IsFailure) return emailResult.Error!;
        
        return new Company(Guid.NewGuid(), name,
            description, emailResult.Value);
    }
    
    public Result<Service> AddService( string name, 
        string description, TimeSpan openingTime,
        TimeSpan closingTime, List<DayOfWeek> workDays,decimal price)
    {
        var service= Service.Create( this.Id, name, description, 
            openingTime, closingTime, workDays, price);
      
        _services.Add(service);
        return service;
    }

    public Result<Ticket> AddTicketToService(Guid serviceId, 
        Guid userId, DateTime startTimeUtc, DateTime endTimeUtc)
    {   
        var service=_services
            .FirstOrDefault(s => s.Id == serviceId);
        
        if (service == null) return ServiceErrors.NotFound;
        
        
        var result = service.AddTicketToService(
            userId, startTimeUtc, endTimeUtc);

        if (result.IsFailure) return result.Error!;

        var ticketEvent= new TicketCreatedDomainEvent(Guid.NewGuid(),
            result.Value.Id);
        RaiseDomainEvent(ticketEvent);
        return result.Value;
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
        var TicketEvent= new TicketCancelledDomainEvent(
            Guid.NewGuid(), ticketId);
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

        var ticketEvent = new TicketSoldDomainEvent(
            Guid.NewGuid(), ticketId);
        
        RaiseDomainEvent(ticketEvent);
        return Result.Success();
    }
}