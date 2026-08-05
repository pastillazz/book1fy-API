using Domain.Entities;

namespace Application.Abstractions.Authentication;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}