using Application.Common.Abstractions.Interfaces;

namespace Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id):IQuery<UserResponse>;