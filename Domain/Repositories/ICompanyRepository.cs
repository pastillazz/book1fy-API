using Domain.Entities;

namespace Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Company company);
    void Remove(Company company);
    Task<Company?>GetCompleteByIdAsync(Guid companyId, Guid serviceId, 
        CancellationToken cancellationToken = default);
}