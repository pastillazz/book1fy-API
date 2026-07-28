using Application.Abstractions.Interfaces;

namespace Application.Companies.Queries.GetCompanyById;

public record GetCompanyByIdQuery(Guid Id):IQuery<CompanyResponse>;