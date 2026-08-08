using Application.Common.Abstractions.Interfaces;
using Application.Users.Queries;

namespace Application.Users.Queries.GetUserByEmail;

public  record GetUserByEmailQuery(string Email):IQuery<UserResponse>;