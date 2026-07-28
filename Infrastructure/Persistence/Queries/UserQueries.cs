using Application.Users.Queries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries;

public class UserQueries(AppDbContext context):IUserQueries
{
    public async Task<UserResponse?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
       return await context.Users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new UserResponse(
                x.Id,
                x.FullName.FirstName,
                x.FullName.LastName,
                x.Username,
                x.Email.Value,
                x.PhoneNumber
            )).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        
        return await context.Users
            .AsNoTracking()
            .Where(x => x.Email.Value == email)
            .Select(x => new UserResponse(
                x.Id,
                x.FullName.FirstName,
                x.FullName.LastName,
                x.Username,
                x.Email.Value,
                x.PhoneNumber
            )).FirstOrDefaultAsync(cancellationToken);
    }
}