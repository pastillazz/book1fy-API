using Domain.Entities;

namespace Domain.Repositories;

public interface ICompanyRepository
{
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Company company);
    void Remove(Company company);
    Task<Company?>GetCompleteByIdAsync(Guid companyId, Guid serviceId, 
        CancellationToken cancellationToken = default);
}