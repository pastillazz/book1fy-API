using Domain.Enums;
using Domain.Primitives;

namespace Domain.Entities;

public class Ticket:Entity
{
    private Ticket(Guid id, Guid serviceId, DateTime startTimeUtc,
        DateTime endTimeUtc, decimal price) : base(id)
    {   
        ServiceId = serviceId;
        StartTimeUtc = startTimeUtc;
        EndTimeUtc = endTimeUtc;
        Status= TicketStatus.Available;
        Price = price;
    }
    
    protected Ticket()
    { }
    public Guid ServiceId { get; private set; }
    public TicketStatus Status { get; private set; }
    public DateTime StartTimeUtc { get; private set; }
    public DateTime EndTimeUtc { get; private set; }
    public Guid? BookingId { get; private set; }
    public decimal Price { get; private set; }

    internal void Reserve(Guid bookingId)
    {
        BookingId = bookingId;
        Status = TicketStatus.Reserved;
    }
    
    internal void CancelReservation()
    {
        BookingId = null;
        Status = TicketStatus.Cancelled;
    }
}