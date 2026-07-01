using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    void Add(User user);
    
    void Update(User user);
    
    void Remove(User user);
}