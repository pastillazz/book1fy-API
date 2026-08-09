using Domain.Entities;

namespace Application.Common.Abstractions.Authentication;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}