using Common.DTOs;

namespace Microsoft.EntityFrameworkCore;

public static class Extensions {
  public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class {
    Throw.IfNull(source, nameof(source));
    pageNumber = pageNumber == 0 ? 1 : pageNumber;
    pageSize = pageSize == 0 ? 10 : pageSize;
    long count = await source.LongCountAsync();
    pageNumber = pageNumber <= 0 ? 1 : pageNumber;
    List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
  }
}


