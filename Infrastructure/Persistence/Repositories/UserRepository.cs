using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(AppWriteDbContext context):IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
       return context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u=> u.Id == id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var emailValue = Email.Reconstruct(email);
        return context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u=> u.Email==emailValue, cancellationToken);
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
