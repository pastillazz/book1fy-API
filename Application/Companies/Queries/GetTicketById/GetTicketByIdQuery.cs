using Application.Abstractions.Interfaces;

namespace Application.Companies.Queries.GetTicketById;

public record GetTicketByIdQuery(
    Guid CompanyId,
    Guid ServiceId,
    Guid TicketId)
    : IQuery<TicketResponse>;
