using Domain.Entities;

namespace Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Company company, CancellationToken cancellationToken = default);
    void Update(Company company);
    void Remove(Company company);
}