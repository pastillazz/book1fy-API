using Domain.Enums;
using Domain.Primitives;

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
    
    protected Ticket()
    { }
    public Guid ServiceId { get; private set; }
    public Guid UserId { get; private set; }
    public TicketStatus Status { get; private set; }
    public DateTime StartTimeUtc { get; private set; }
    public DateTime EndTimeUtc { get; private set; }
    
    public decimal Price { get; private set; }
    
    internal static Ticket Create(Guid serviceId, Guid userId, DateTime startTimeUtc,
        DateTime endTimeUtc, decimal price)
    {   
        return new Ticket(Guid.NewGuid(), userId, serviceId, startTimeUtc, endTimeUtc, price);
    }
    internal void CancelReservation()
    {
        Status = TicketStatus.Cancelled;
    }
    internal void SellReservation()
    {
        Status = TicketStatus.Sold;
    }
    
}