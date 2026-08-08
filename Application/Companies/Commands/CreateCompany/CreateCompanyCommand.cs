using Application.Common.Abstractions.Interfaces;


namespace Application.Companies.Commands.CreateCompany;

public sealed record CreateCompanyCommand(
    string Name,
    string Description,
    string Email
    ):ICommand<Guid>;