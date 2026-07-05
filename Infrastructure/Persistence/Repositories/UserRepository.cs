using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context):IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
       return context.Users
            .FirstOrDefaultAsync(u=> u.Id == id, cancellationToken);
    }
    
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return context.Users
            .FirstOrDefaultAsync(u=> u.Email.Value == email, cancellationToken);
    }

    public void Add(User user)
    {
        context.Users.Add(user);
    }
    
    public void Remove(User user)
    {
        context.Users.Remove(user);
    }
}
