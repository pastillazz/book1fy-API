using Application.Users.Queries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries;

public class UserQueries(AppReadDbContext context):IUserQueries
{
    public async Task<UserResponse?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
       return await context.Users
            .Where(x => x.Id == id)
            .Select(x => new UserResponse(
                x.Id,
                x.FirstName,
                x.LastName,
                x.Username,
                x.Email,
                x.PhoneNumber
            )).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        
        return await context.Users
            .Where(x => x.Email == email)
            .Select(x => new UserResponse(
                x.Id,
                x.FirstName,
                x.LastName,
                x.Username,
                x.Email,
                x.PhoneNumber
            )).FirstOrDefaultAsync(cancellationToken);
    }
}