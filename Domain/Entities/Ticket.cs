using Domain.Abstractions;
using Domain.Enums;
using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public class Ticket:Entity
{
    private Ticket(Guid id, Guid userId, Guid serviceId, DateTime startTimeUtc,
        DateTime endTimeUtc, decimal price) : base(id)
    {   UserId= userId;
        ServiceId = serviceId;
        StartTimeUtc = startTimeUtc;
        EndTimeUtc = endTimeUtc;
        Status= TicketStatus.Reserved;
        Price = price;
    }
    
    private Ticket()
    { }
    public Guid ServiceId { get; private set; }
    public Guid UserId { get; private set; }
    public TicketStatus Status { get; private set; }
    public DateTime StartTimeUtc { get; private set; }
    public DateTime EndTimeUtc { get; private set; }
    
    public decimal Price { get; private set; }
    
    internal static Ticket Create(Guid serviceId, Guid userId, 
        DateTime startTimeUtc, DateTime endTimeUtc, decimal price)
    {   
        return new Ticket(Guid.NewGuid(), userId, serviceId,
            startTimeUtc, endTimeUtc, price);
    }
    internal Result CancelReservation()
    {
        if (Status is not TicketStatus.Reserved) 
            return TicketErrors.InvalidStatus;
        
        Status = TicketStatus.Cancelled;
        return Result.Success();
    }

    internal Result SellReservation()
    {
        if (Status is not TicketStatus.Reserved)
            return TicketErrors.InvalidStatus;
        
        Status = TicketStatus.Sold;
        return Result.Success();
    }

}