using Domain.Enums;
using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public sealed class Service : Entity
{   private readonly List<DayOfWeek> _workDays=new();
    private readonly List<Ticket> _tickets=new();
    
    private Service(Guid id, Guid companyId, string name, string description,
        TimeSpan openingTime, TimeSpan closingTime, List<DayOfWeek> workDays,
        decimal price) : base(id)
    {
        CompanyId = companyId;
        Name = name;
        Description = description;
        OpeningTime = openingTime;
        ClosingTime = closingTime;
        _workDays = workDays.ToList();
        Price = price;
    }

    private Service()
    {
        Name = null!;
        Description = null!;
    }
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Decimal Price { get; private set; }
    public TimeSpan OpeningTime { get; private set; }
    public TimeSpan ClosingTime { get; private set; }
    public IReadOnlyCollection<DayOfWeek> WorkDays => _workDays;
    public IReadOnlyCollection<Ticket> Tickets => _tickets;
    
    internal static Result<Service> Create(
        Guid companyId, string name, string description,
        TimeSpan openingTime, TimeSpan closingTime,
        List<DayOfWeek> workDays,
        decimal price)
    {
        var scheduleValidation = IsScheduleValid(
            openingTime, closingTime, workDays, price);

        if (scheduleValidation.IsFailure)
            return scheduleValidation.Error;

        if (string.IsNullOrWhiteSpace(name))
            return ServiceErrors.NameEmpty;

        return new Service(Guid.NewGuid(), companyId, name.Trim(),
            description?.Trim() ?? string.Empty,
            openingTime, closingTime,
            workDays.Distinct().ToList(), price);
    }

    private static Result IsScheduleValid(
        TimeSpan openingTime, TimeSpan closingTime,
        List<DayOfWeek> workDays, decimal price)
    {
        if (openingTime < TimeSpan.Zero || closingTime > TimeSpan.FromDays(1))
            return ServiceErrors.ScheduleOutOfRange;

        if (openingTime >= closingTime)
            return ServiceErrors.InvalidSchedule;

        if (workDays is null || workDays.Count == 0)
            return ServiceErrors.WorkDaysEmpty;

        if (price < 0)
            return ServiceErrors.NegativePrice;

        return Result.Success();
    }

    internal Result<Ticket> AddTicketToService(
        Guid userId,
        DateTime startTimeUtc, DateTime endTimeUtc)
    {
        var ticketValidation = IsTicketValid(
            startTimeUtc, endTimeUtc);

        if (!ticketValidation.IsSuccess) 
            return ticketValidation.Error;
        
        var ticket = Ticket.Create(Id, userId,
            startTimeUtc, endTimeUtc, Price);
        _tickets.Add(ticket);
        return ticket;
    }

    private Result IsTicketValid(
        DateTime startTimeUtc, DateTime endTimeUtc)
    {
        if (endTimeUtc <= startTimeUtc) 
            return TicketErrors.InvalidTimes;
        
        if (!WorkDays.Contains(startTimeUtc.DayOfWeek)) 
            return TicketErrors.InvalidDay;
        
        if (startTimeUtc.TimeOfDay < OpeningTime ||
            endTimeUtc.TimeOfDay > ClosingTime) 
            return TicketErrors.InvalidTimes;
        

        if (_tickets.
            Where(t => t.Status == TicketStatus.Reserved).
            Any(t => t.StartTimeUtc < endTimeUtc
                     && t.EndTimeUtc > startTimeUtc)) 
            return TicketErrors.OverlappingTicket;

        
        return Result.Success();
    }

    internal Result CancelTicket(Guid ticketId)
    {
        var ticket = _tickets
            .FirstOrDefault(t => t.Id == ticketId);
        
        if (ticket is null) 
            return TicketErrors.NotFound;
        
        var result = ticket.CancelReservation();
        if (result.IsFailure) 
            return result.Error;
        
        return Result.Success();
    }

    internal Result SellTicket(Guid ticketId)
    {
        var ticket = _tickets
            .FirstOrDefault(t => t.Id == ticketId);
        if (ticket is null)
            return TicketErrors.NotFound;
        

        var result = ticket.SellReservation();
        if (result.IsFailure) 
            return result.Error;
        
        return Result.Success();
    }

}
    