
using Application.Abstractions.Interfaces;
using ICommand = Application.Abstractions.Interfaces.ICommand;

namespace Application.Companies.Commands.CreateCompany;

public sealed record CreateCompanyCommand(
    string Name,
    string Description,
    string Email
    ):ICommand<Guid>;