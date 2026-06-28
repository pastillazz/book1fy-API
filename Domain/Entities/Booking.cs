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
 
}