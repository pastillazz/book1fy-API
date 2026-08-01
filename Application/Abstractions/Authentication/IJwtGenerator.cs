using Domain.Entities;

namespace Application.Abstractions.Authentication;

public interface IJwtGenerator
{
    string Generate(User user);
}