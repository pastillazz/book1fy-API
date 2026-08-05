using Domain.Abstractions;

namespace Application.Companies.Queries;

public interface ICompanyQueries
{
    Task<CompanyResponse?> GetCompanyByIdAsync
        (Guid id, CancellationToken cancellationToken);
    Task<CompanyResponse?> GetCompanyByEmailAsync
        (string email, CancellationToken cancellationToken);
    Task<ServiceResponse?> GetServiceByIdAsync
        (Guid companyId, Guid serviceId, CancellationToken cancellationToken);
    Task<TicketResponse?> GetTicketByIdAsync
        (Guid companyId, Guid serviceId, Guid ticketId,
            CancellationToken cancellationToken);
}