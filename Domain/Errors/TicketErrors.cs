using System.Net;
using Domain.Abstractions;

namespace Domain.Errors;

public class TicketErrors
{
    public static readonly Error NotFound = new ("Ticket.NotFound", 
        "Ticket was not found.",HttpStatusCode.NotFound);
    
    public static readonly Error InvalidTimes=new("Ticket.InvalidTimes",
        "Ticket times are outside of service hours.",
        HttpStatusCode.Conflict);

    public static readonly Error InvalidDay=new("Ticket.InvalidDay",
        "The service does not operate on the requested day.",
        HttpStatusCode.Conflict);

    public static readonly Error InvalidStatus=new("Ticket.InvalidStatus",
        "The ticket cannot transition from its current status.",
        HttpStatusCode.Conflict);

    public static readonly Error OverlappingTicket=new("Ticket.OverlappingTicket",
        "Ticket times overlap with an existing ticket.",
        HttpStatusCode.Conflict);
}