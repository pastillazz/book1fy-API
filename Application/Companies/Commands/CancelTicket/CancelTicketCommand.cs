using Application.Common.Abstractions.Interfaces;

namespace Application.Companies.Commands.CancelTicket;

public record CancelTicketCommand(
    Guid CompanyId,
    Guid ServiceId,
    Guid TicketId
    ):ICommand;