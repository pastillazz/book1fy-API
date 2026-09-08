using Application.Common.Abstractions.Interfaces;
using Application.Common.Abstractions.Link;
using Application.Companies.Queries.Interfaces;
using Application.Companies.Queries.Responses;
using Domain.Shared;

namespace Application.Companies.Queries.GetServices;

public class GetServicesQueryHandler:
    IQueryHandler<GetServicesQuery, PagedList<ServiceResponse>>
{
    private readonly ICompanyQueries _companyQueries;
    private readonly ILinkService _linkService;
    public GetServicesQueryHandler(ICompanyQueries companyQueries,
        ILinkService linkService)
    {
        _companyQueries = companyQueries;
        _linkService = linkService;
    }

    public async Task<Result<PagedList<ServiceResponse>>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
    {
        var services = await _companyQueries
            .GetAllServicesAsync(request.SearchTerm, request.SortColumn,
                request.SortOrder, request.Page, request.PageSize,
                cancellationToken);
        
        if (services?.Items is null || !services.Items.Any())
        {
            return PagedList<ServiceResponse>
                .Create(new List<ServiceResponse>(), 0, 
                    request.Page, request.PageSize);
        }
        
        AddLinks(services, request);
        return services;
    }

    private void AddLinks(PagedList<ServiceResponse> services, GetServicesQuery query)
    {
        services.Links.Add(_linkService.Generate("GetServices",
            new
            {
                searchTerm=query.SearchTerm,
                sortColumn=query.SortColumn,
                sortOrder=query.SortOrder,
                page=query.Page,
                pageSize=query.PageSize
            },
            "self",
            "GET"
        ));
        
        if (services.HasNextPage)
        {
            services.Links.Add(_linkService.Generate("GetServices",
                new
                {
                    searchTerm=query.SearchTerm,
                    sortColumn=query.SortColumn,
                    sortOrder=query.SortOrder,
                    page=query.Page+1,
                    pageSize=query.PageSize
                },
                "next-page",
                "GET"
            ));
        }

        if (services.HasPreviousPage)
        {
            services.Links.Add(_linkService.Generate("GetServices",
                new
                {
                    searchTerm=query.SearchTerm,
                    sortColumn=query.SortColumn,
                    sortOrder=query.SortOrder,
                    page=query.Page-1,
                    pageSize=query.PageSize
                },
                "previous-page",
                "GET"
            ));
        }
    }
}