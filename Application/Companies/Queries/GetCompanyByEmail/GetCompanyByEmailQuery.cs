using Application.Abstractions.Interfaces;

namespace Application.Companies.Queries.GetCompanyByEmail;

public record GetCompanyByEmailQuery(string Email):IQuery<CompanyResponse>;