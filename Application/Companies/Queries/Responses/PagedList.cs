using Application.Common.Abstractions.Link;

namespace Application.Companies.Queries.Responses;

public class PagedList<T>
{
    private PagedList(List<T> items, int count, int page, int pageSize)
    {
        Items = items;
        TotalCount = count;
        Page = page;
        PageSize = pageSize;
    }
    public List<T> Items { get; } 
    public int TotalCount { get; }
    public int Page { get; }
    public int PageSize { get; }
    public bool HasNextPage=> Page* PageSize < TotalCount;
    public bool HasPreviousPage=> Page > 1;
    
    public List<Link> Links { get; set; } = new List<Link>();
    
    public static PagedList<T> Create
        (List<T> items, int totalCount, int page, int pageSize)
    {
        return new (items, totalCount, page, pageSize);
    }
}