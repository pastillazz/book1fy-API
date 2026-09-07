using Application.Common.Abstractions.Interfaces;
using Application.Companies.Queries.Responses;

namespace Application.Companies.Queries.GetServices;

public record GetServicesQuery(string? SearchTerm,
    string? SortColumn, string? SortOrder,
    int Page, int PageSize) :
    IQuery<PagedList<ServiceResponse>>;