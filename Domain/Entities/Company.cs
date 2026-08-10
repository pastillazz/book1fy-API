using Domain.DomainEvents;
using Domain.Enums;
using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Company:AggregateRoot
{   
    private readonly List<Service> _services=new();
    private Company(Guid id, string name, string description,
        Email email, Guid ownerId ) : base(id)
    {
        Name = name;
        Description = description;
        Email = email;
        OwnerId = ownerId;
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
    public Guid OwnerId { get; private set; }
    public CompanyStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<Service> Services => _services;

    public static Result<Company> Create( string name,
        string description, string email, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return CompanyErrors.NameEmpty;

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure) return emailResult.Error;

        var company = new Company(Guid.NewGuid(), name.Trim(),
            description?.Trim() ?? string.Empty, emailResult.Value, ownerId);

        var companyEvent = new CompanyCreatedDomainEvent(
            Guid.NewGuid(),
            company.Id,
            company.Name,
            company.Email.Value,
            company.OwnerId);

        company.RaiseDomainEvent(companyEvent);
        return company;
    }
    
    public Result<Service> AddService( string name,
        string description, TimeSpan openingTime,
        TimeSpan closingTime, List<DayOfWeek> workDays,decimal price)
    {
        var result= Service.Create( this.Id, name, description,
            openingTime, closingTime, workDays, price);

        if (result.IsFailure) return result.Error;

        _services.Add(result.Value);
        return result.Value;
    }

    public Result<Ticket> AddTicketToService(Guid serviceId, 
        Guid userId, DateTime startTimeUtc, DateTime endTimeUtc)
    {   
        var service=_services
            .FirstOrDefault(s => s.Id == serviceId);
        
        if (service == null) return ServiceErrors.NotFound;
        
        
        var result = service.AddTicketToService(
            userId, startTimeUtc, endTimeUtc);

        if (result.IsFailure) return result.Error;

        var ticket = result.Value;

        var ticketEvent= new TicketCreatedDomainEvent(
            Guid.NewGuid(),
            ticket.Id,
            this.Id,
            this.Name,
            this.Email.Value,
            service.Id,
            service.Name,
            ticket.UserId,
            ticket.StartTimeUtc,
            ticket.EndTimeUtc,
            ticket.Price);

        RaiseDomainEvent(ticketEvent);
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
            return result.Error;
        }

        var ticket = result.Value;

        var ticketEvent= new TicketCancelledDomainEvent(
            Guid.NewGuid(),
            ticket.Id,
            this.Id,
            this.Name,
            this.Email.Value,
            service.Id,
            service.Name,
            ticket.UserId,
            ticket.StartTimeUtc,
            ticket.EndTimeUtc,
            ticket.Price);

        RaiseDomainEvent(ticketEvent);
        return Result.Success();
    }

}