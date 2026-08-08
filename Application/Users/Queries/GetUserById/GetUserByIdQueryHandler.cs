using Application.Common.Abstractions.Interfaces;
using Domain.Errors;
using Domain.Shared;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserQueries userQueries) : 
    IQueryHandler<GetUserByIdQuery,UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user= await userQueries.GetUserByIdAsync(request.Id,cancellationToken);

        if (user is null) return UserErrors.UserNotFound;

        return user;
    }
}
