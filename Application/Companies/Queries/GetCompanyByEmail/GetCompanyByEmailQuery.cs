using Application.Abstractions.Interfaces;
using Application.Companies.Queries.Responses;

namespace Application.Companies.Queries.GetCompanyByEmail;

public record GetCompanyByEmailQuery(string Email):IQuery<CompanyResponse>;