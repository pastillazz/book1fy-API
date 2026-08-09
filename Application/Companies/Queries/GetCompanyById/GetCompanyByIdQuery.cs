using Application.Common.Abstractions.Interfaces;
using Application.Companies.Queries.Responses;

namespace Application.Companies.Queries.GetCompanyById;

public record GetCompanyByIdQuery(Guid Id):IQuery<CompanyResponse>;