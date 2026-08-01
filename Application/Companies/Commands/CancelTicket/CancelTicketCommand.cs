using Application.Abstractions.Interfaces;
using Domain.Abstractions;

namespace Application.Companies.Commands.CancelTicket;

public record CancelTicketCommand(
    Guid CompanyId,
    Guid ServiceId,
    Guid TicketId
    ):ICommand;