using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;

namespace BeautySaloon.API.Helpers;

public static class PaginationHelper
{
    public static (List<T> PageItems, int TotalPages) PaginateAndSort<T>(
    List<T> items,
    Func<T, IComparable> sortBy,
    int pageNumber,
    int pageSize,
    bool isDescending = false)
    {
        int totalItems = items.Count;
        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        var sortedItems = isDescending ? items.OrderByDescending(sortBy) : items.OrderBy(sortBy);

        var pageItems = sortedItems
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (pageItems, totalPages);
    }
}
