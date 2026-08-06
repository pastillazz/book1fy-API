using Application.Companies.Queries.Responses;

namespace Application.Companies.Queries.Interfaces;

public interface ICompanyQueries
{
    Task<CompanyResponse?> GetCompanyByIdAsync
        (Guid id, CancellationToken cancellationToken);
    Task<ServiceResponse?> GetServiceByIdAsync
        (Guid companyId, Guid serviceId, CancellationToken cancellationToken);
    Task<TicketResponse?> GetTicketByIdAsync
        (Guid companyId, Guid serviceId, Guid ticketId,
            CancellationToken cancellationToken);
}