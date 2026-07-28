using Domain.Abstractions;

namespace Application.Companies.Queries;

public interface ICompanyQueries
{
    Task<CompanyResponse?> GetCompanyByIdAsync
        (Guid id, CancellationToken cancellationToken);
    Task<CompanyResponse?> GetCompanyByEmailAsync
        (string email, CancellationToken cancellationToken);
}