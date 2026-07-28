

namespace Application.Users.Queries;

public interface IUserQueries
{
    Task<UserResponse?> GetUserByIdAsync
        (Guid id, CancellationToken cancellationToken);
    Task<UserResponse?> GetUserByEmailAsync
        (string email, CancellationToken cancellationToken);
}