namespace People.Application.Models;

public class PagedList<T>
{
    public int PageSize { get; set; } = 20;
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<T> List { get; set; }

    public bool HasPreviousPage => (Page > 1);

    public bool HasNextPage => (Page < TotalPages);

    public PagedList()
    {
        List = new List<T>();
    }
        
    public PagedList(int pageSize, int page, int totalCount,  IEnumerable<T> subset)
    {
        List = new List<T>();
            
        PageSize = pageSize;
        Page = page;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        List.AddRange(subset);
    }
        
    public override string ToString()
    {
        if(TotalCount == 0)
        {
            return "Records not found";
        }

        if(Page == 1 && List.Count < PageSize)
        {
            return $"{List.Count} results";
        }

        return $"{((Page - 1) * PageSize) + 1}-{(Page - 1) * PageSize + List.Count} of {TotalCount:N0}";
    }
}