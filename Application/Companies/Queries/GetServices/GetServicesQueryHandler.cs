using Application.Common.Abstractions.Interfaces;
using Application.Companies.Queries.Interfaces;
using Application.Companies.Queries.Responses;
using Domain.Shared;

namespace Application.Companies.Queries.GetServices;

public class GetServicesQueryHandler(ICompanyQueries companyQueries):
    IQueryHandler<GetServicesQuery, PagedList<ServiceResponse>>
{
    public async Task<Result<PagedList<ServiceResponse>>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
    {
        var services = await companyQueries
            .GetAllServicesAsync(request.SearchTerm, request.SortColumn,
                request.SortOrder, request.Page, request.PageSize,
                cancellationToken);
        
        if (services?.Items is null || !services.Items.Any())
        {
            return PagedList<ServiceResponse>
                .Create(new List<ServiceResponse>(), 0, 
                    request.Page, request.PageSize);
        }

        return services;
    }
}