using Common.Data;

namespace Microsoft.EntityFrameworkCore;

public static class Extensions {
  public static async Task<PagedListResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class {
    Throw.IfNull(source, nameof(source));
    pageNumber = pageNumber == 0 ? 1 : pageNumber;
    pageSize = pageSize == 0 ? 10 : pageSize;
    var count = await source.LongCountAsync();
    pageNumber = pageNumber <= 0 ? 1 : pageNumber;
    var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    return PagedListResult<T>.Success(items, count, pageNumber, pageSize);
  }
}


