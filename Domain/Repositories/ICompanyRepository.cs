using Domain.Entities;

namespace Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Company company, CancellationToken cancellationToken = default);
    void Remove(Company company);
    Task<bool>HasTicketExists(Guid serviceId, DateTime startTimeUtc, 
        DateTime endTimeUtc, CancellationToken cancellationToken = default);
}