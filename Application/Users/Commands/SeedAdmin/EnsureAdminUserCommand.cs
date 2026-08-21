using Application.Common.Abstractions.Interfaces;

namespace Application.Users.Commands.SeedAdmin;

public record EnsureAdminUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    string PhoneNumber):ICommand<Guid>
{

}
