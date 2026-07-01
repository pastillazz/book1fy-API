using Domain.Abstractions;

namespace Domain.ValueObjects.Errors;

public class TicketErrors
{
    public static readonly Error NotFound = new ("Ticket.NotFound", 
        "Ticket was not found.");
}