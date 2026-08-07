using Application.Abstractions.Interfaces;
using Application.Companies.Queries.Interfaces;
using Application.Companies.Queries.Responses;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Companies.Queries.GetTicketById;

public class GetTicketByIdQueryHandler(ICompanyQueries companyQueries)
    : IQueryHandler<GetTicketByIdQuery, TicketResponse>
{
    public async Task<Result<TicketResponse>> Handle(
        GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var ticket = await companyQueries.GetTicketByIdAsync(
            request.CompanyId, request.ServiceId, request.TicketId,
            cancellationToken);

        if (ticket is null) return TicketErrors.NotFound;

        return ticket;
    }
}
