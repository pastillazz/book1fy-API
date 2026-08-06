namespace Api.Companies.Ticket;

public record CreateTicketRequest(
    DateTime StartTimeUtc,
    DateTime EndTimeUtc);
