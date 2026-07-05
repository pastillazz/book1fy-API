
using ICommand = Application.Abstractions.Interfaces.ICommand;

namespace Application.Companies.Commands;

public sealed record CreateCompanyCommand(
    Guid Id,
    string Name,
    string Description
    ):ICommand;