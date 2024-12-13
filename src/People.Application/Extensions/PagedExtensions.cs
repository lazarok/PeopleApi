using Microsoft.EntityFrameworkCore;
using People.Application.Models;

namespace People.Application.Extensions;

public static class PagedExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, PaginationFilter filter) where T : class
    {
        var pagedList = new PagedList<T>();
        pagedList.Page = filter.PageNumber;
        pagedList.PageSize = filter.PageSize;
        pagedList.TotalCount = await source.CountAsync();
        pagedList.TotalPages = (int)Math.Ceiling(pagedList.TotalCount / (double)pagedList.PageSize);

        var items = await source.Skip((filter.PageNumber - 1) * pagedList.PageSize).Take(pagedList.PageSize).ToListAsync();
        pagedList.List.AddRange(items);
            
        return pagedList;
    }
    
    public static PagedList<T> ToPagedList<T>(this List<T> source, PaginationFilter filter) where T : class
    {
        var pagedList = new PagedList<T>();
        pagedList.Page = filter.PageNumber;            
        pagedList.PageSize = filter.PageSize;
        pagedList.TotalCount = source.Count();
        pagedList.TotalPages = (int)Math.Ceiling(pagedList.TotalCount / (double)pagedList.PageSize);

        var items = source.Skip((filter.PageNumber - 1) * pagedList.PageSize).Take(pagedList.PageSize).ToList();
        pagedList.List.AddRange(items);
            
        return pagedList;
    }
}