using System.Linq.Expressions;

namespace Common.Collections.Paged;
public static class PagedListExtensions {
  public static async Task<List<int>> CountAsync<T>(this IEnumerable<T> superset, CancellationToken cancellationToken) => await Task.Run(superset.Count, cancellationToken);
  public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> superset, int numberOfPages) {
    int take = Convert.ToInt32(Math.Ceiling(superset.Count() / (double)numberOfPages));
    var result = new List<IEnumerable<T>>();
    for (int i = 0; i < numberOfPages; i++) {
      var chunk = superset.Skip(i * take).Take(take).ToList();
      if (chunk.Any()) {
        result.Add(chunk);
      };
    }
    return result;
  }

  public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> superset, int pageSize) {
    var count = superset.Count();
    if (count < pageSize) {
      yield return superset;
    } else {
      var numberOfPages = Math.Ceiling(count / (double)pageSize);
      for (var i = 0; i < numberOfPages; i++) {
        yield return superset.Skip(pageSize * i).Take(pageSize);
      }
    }
  }

  public static IQueryable<T> SkipToPage<T>(this IQueryable<T> query, int pageNumber, int pageSize) => query.Skip(pageNumber > 1 ? pageNumber - 1 : 0).Take(pageSize);

  public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset, int pageNumber, int pageSize) {
    return new PagedList<T>(superset, pageNumber, pageSize);
  }

  public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset) {
    int supersetSize = superset.Count();
    int pageSize = supersetSize == 0 ? 1 : supersetSize;
    return new PagedList<T>(superset, 1, pageSize);
  }

  public static IPagedList<TResult> Select<TSource, TResult>(this IPagedList<TSource> source, Func<TSource, TResult> selector) {
    var subset = ((IEnumerable<TSource>)source).Select(selector);
    return new PagedList<TResult>(source, subset);
  }

  public static IPagedList<T> ToPagedList<T, TKey>(this IQueryable<T> superset, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize) {
    return new PagedList<T, TKey>(superset, keySelector, pageNumber, pageSize);
  }

  public static IPagedList<T> ToPagedList<T, TKey>(this IEnumerable<T> superset, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize) {
    return new PagedList<T, TKey>(superset.AsQueryable(), keySelector, pageNumber, pageSize);
  }

  public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> superset) => await ToListAsync(superset, CancellationToken.None);

  public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> superset, CancellationToken cancellationToken) => await Task.Run(superset.ToList, cancellationToken);

  public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize, CancellationToken cancellationToken) {
    if (pageNumber < 1) {
      throw new ArgumentOutOfRangeException($"pageNumber = {pageNumber}. PageNumber cannot be below 1.");
    }
    if (pageSize < 1) {
      throw new ArgumentOutOfRangeException($"pageSize = {pageSize}. PageSize cannot be less than 1.");
    }
    var subset = new List<T>();
    var totalCount = 0;
    if (superset != null) {
      totalCount = superset.CountAsync(cancellationToken);
      if (totalCount > 0) {
        subset.AddRange(await superset.SkipToPage(pageNumber, pageSize).ToListAsync(cancellationToken).ConfigureAwait(false));
      }
    }
    return new StaticPagedList<T>(subset, pageNumber, pageSize, totalCount);
  }

  public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize) {
    return await ToPagedListAsync(superset, pageNumber, pageSize, CancellationToken.None);
  }

  public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> superset, int pageNumber, int pageSize, CancellationToken cancellationToken) {
    return await ToPagedListAsync(superset.AsQueryable(), pageNumber, pageSize, cancellationToken);
  }

  public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> superset, int pageNumber, int pageSize) {
    return await ToPagedListAsync(superset.AsQueryable(), pageNumber, pageSize, CancellationToken.None);
  }

  public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int? pageNumber, int pageSize, CancellationToken cancellationToken) {
    return await ToPagedListAsync(superset.AsQueryable(), pageNumber ?? 1, pageSize, cancellationToken);
  }
  public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int? pageNumber, int pageSize) {
    return await ToPagedListAsync(superset.AsQueryable(), pageNumber ?? 1, pageSize, CancellationToken.None);
  }

}
