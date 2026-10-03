using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(AppWriteDbContext context):IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
       return await context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u=> u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var emailValue = Email.Reconstruct(email);
        return await context.Users
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

    public void Add(RefreshToken refreshToken)
    {
        context.Set<RefreshToken>().Add(refreshToken);
    }
     public async Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await context
      .Set<RefreshToken>()
      .Include(rt => rt.User)
      .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> DeleteRefreshTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await context.Set<RefreshToken>()
            .ExecuteDeleteAsync( cancellationToken);
        
        return true;
    }
}
