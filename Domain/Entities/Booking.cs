using Domain.Enums;
using Domain.Primitives;

namespace Domain.Entities;

public class Booking : AggregateRoot
{
    private Booking(Guid id, Guid userId ,Guid ticketId, decimal totalAmount) : base(id)
    {
        UserId = userId;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
        Status = BookingStatus.Pending;
        TotalAmount = totalAmount;
        TicketId = ticketId;
    }
    public Guid UserId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public BookingStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public Guid TicketId { get; private set; }
    
    public static Booking Create(Guid id, Guid userId, Guid ticketId, decimal totalAmount)
    {   
        return new Booking(id, userId, ticketId, totalAmount);
    }

    public static Booking Complete(Booking booking)
    {
        booking.Status = BookingStatus.Completed;
        booking.UpdatedAtUtc = DateTime.UtcNow;
        return booking;
    }
}