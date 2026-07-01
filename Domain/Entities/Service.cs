using Domain.Abstractions;
using Domain.Primitives;
using Domain.ValueObjects.Errors;

namespace Domain.Entities;

public sealed class Service : Entity
{   private readonly List<DayOfWeek> _workDays=new();
    private readonly List<Ticket> _tickets=new();
    
    internal Service(Guid id, Guid tenantId, string name, string description,
        TimeSpan openingTime, TimeSpan closingTime, List<DayOfWeek> workDays,
        decimal price) : base(id)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        OpeningTime = openingTime;
        ClosingTime = closingTime;
        _workDays = workDays;
        Price = price;
    }

    protected Service()
    {
    }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Decimal Price { get; private set; }
    public TimeSpan OpeningTime { get; private set; }
    public TimeSpan ClosingTime { get; private set; }
    public IReadOnlyCollection<DayOfWeek> WorkDays => _workDays;
    public IReadOnlyCollection<Ticket> Tickets => _tickets;
    
    internal static Service Create(Guid id, Guid tenantId, string name, string description,
        TimeSpan openingTime, TimeSpan closingTime, List<DayOfWeek> workDays,
        decimal price)
    {
        return new Service(id, tenantId, name, description,
            openingTime, closingTime, workDays, price);
    }
    internal Ticket AddTicketToService(Guid serviceId, Guid userId, 
        DateTime startTimeUtc, DateTime endTimeUtc)
    {
        var ticket = Ticket.Create(this.Id,userId, startTimeUtc, endTimeUtc, this.Price);
        _tickets.Add(ticket);
        return ticket;
    }

    internal Result CancelTicket(Guid ticketId)
    {
        var ticket = _tickets.FirstOrDefault(t => t.Id == ticketId);
        if (ticket == null)
        {
            return TicketErrors.NotFound;
        }
        ticket.CancelReservation();
        _tickets.Remove(ticket);
        return Result.Success();
    }

    internal Result SellTicket(Guid ticketId)
    {
        var ticket = _tickets.FirstOrDefault(t => t.Id == ticketId);
        if (ticket == null)
        {
           return TicketErrors.NotFound;
        }

        ticket.SellReservation();
        _tickets.Remove(ticket);
        return Result.Success();
    }

}
    