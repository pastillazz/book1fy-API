using Application.Common.Abstractions.Link;

namespace Api.Services;

public class LinkService:ILinkService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LinkGenerator _linkGenerator;
    
    public LinkService(IHttpContextAccessor httpContextAccessor, 
        LinkGenerator linkGenerator)
    {
        _httpContextAccessor = httpContextAccessor;
        _linkGenerator = linkGenerator;
    }
    
    public Link Generate(string endpointName, object routeValues, string rel,
        string method)
    {
        return new Link(
            _linkGenerator.GetUriByName(_httpContextAccessor.HttpContext,
                endpointName,routeValues),
            rel,
            method
        );
    }
}