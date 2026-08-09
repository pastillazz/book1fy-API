using Application.Common.Abstractions.Interfaces;
using Domain.Errors;
using Domain.Shared;

namespace Application.Users.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler(IUserQueries userQueries) : 
    IQueryHandler<GetUserByEmailQuery,UserResponse>
{

    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user= await userQueries.GetUserByEmailAsync(request.Email,cancellationToken);

        if (user is null) return UserErrors.UserNotFound;

        return user;
    }
}
