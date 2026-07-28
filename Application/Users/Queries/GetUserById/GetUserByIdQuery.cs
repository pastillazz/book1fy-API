using Application.Abstractions.Interfaces;

namespace Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id):IQuery<UserResponse>;