namespace Api.Companies.Ticket;

public record CreateTicketRequest(
    Guid UserId,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc);
