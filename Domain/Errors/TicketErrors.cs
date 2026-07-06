using Domain.Abstractions;

namespace Domain.ValueObjects.Errors;

public class TicketErrors
{
    public static readonly Error NotFound = new ("Ticket.NotFound", 
        "Ticket was not found.");
    
    public static readonly Error InvalidTimes=new("Ticket.InvalidTimes",
        "Ticket times are outside of service hours.");
    
    public static readonly Error OverlappingTicket=new("Ticket.OverlappingTicket",
        "Ticket times overlap with an existing ticket.");
}