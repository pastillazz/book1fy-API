using Application.Abstractions.Interfaces;
using Application.Companies.Queries.Responses;

namespace Application.Companies.Queries.GetTicketById;

public record GetTicketByIdQuery(
    Guid CompanyId,
    Guid ServiceId,
    Guid TicketId)
    : IQuery<TicketResponse>;
