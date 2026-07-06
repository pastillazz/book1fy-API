using Application.Abstractions.Interfaces;

namespace Application.Companies.Commands.AddTicket;

public sealed record AddTicketCommand(
    Guid Id, 
    Guid UserId,
    Guid CompanyId,
    Guid ServiceId,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc) : ICommand;