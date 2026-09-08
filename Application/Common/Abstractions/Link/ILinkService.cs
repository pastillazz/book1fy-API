namespace Application.Common.Abstractions.Link;

public interface ILinkService
{
    Link Generate(string endpointName, object routeValues, string rel, string method);
}