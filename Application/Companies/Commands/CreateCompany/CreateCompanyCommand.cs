
using ICommand = Application.Abstractions.Interfaces.ICommand;

namespace Application.Companies.Commands.CreateCompany;

public sealed record CreateCompanyCommand(
    Guid Id,
    string Name,
    string Description
    ):ICommand;