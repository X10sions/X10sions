using System.Linq.Expressions;

namespace Common.Collections.Paged;

public interface IPagedList<T> : IPagedListOptions, IList<T> { }

public static class IPagedListExtensions {

  //public static async Task<int> CountAsync<T>(this IEnumerable<T> superset, CancellationToken cancellationToken = default) => await Task.Run(superset.Count, cancellationToken);
  //public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> superset, CancellationToken cancellationToken = default) => await Task.Run(superset.ToList, cancellationToken);

  public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> superset, int numberOfPages) {
    int take = Convert.ToInt32(Math.Ceiling(superset.Count() / (double)numberOfPages));
    var result = new List<IEnumerable<T>>();
    for (int i = 0; i < numberOfPages; i++) {
      var chunk = superset.Skip(i * take).Take(take).ToList();
      if (chunk.Any()) {
        result.Add(chunk);
      }
    }
    return result;
  }

  public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> superset, int? pageSize = null) {
    var count = superset.Count();
    if (count < pageSize) {
      yield return superset;
    } else {
      var numberOfPages = Math.Ceiling(count / (double)pageSize);
      for (var i = 0; i < numberOfPages; i++) {
        yield return superset.Skip(pageSize.Value * i).Take(pageSize.Value);
      }
    }
  }


  public static PagedList<T> ToPagedList<T>(this IEnumerable<T> items, int pageNumber = 1, int? pageSize = null)
    => new PagedList<T>(items, pageNumber, pageSize, items.Count());

  //public static Task<List<TSource>> ToListAsync<TSource>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken)) {

  public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query
    , Func<IQueryable<T>, CancellationToken, Task<int>> countAsyncFunc
    , Func<IQueryable<T>, CancellationToken, Task<List<T>>> toListAsyncFunc
    , int pageNumber, int? pageSize = null, CancellationToken token = default)
    => new PagedList<T>(await toListAsyncFunc(query.SkipTakeToPage(pageNumber, pageSize), token), pageNumber, pageSize, await countAsyncFunc.Invoke(query, token));

  //=> new PagedList<T>(await query.SkipTakeToPage(pageNumber, pageSize).ToListAsync(token), pageNumber, pageSize, await countAsyncFunc() await query.CountAsync(token));

  public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int? pageNumber, int? pageSize = null, CancellationToken token = default)
    => await ToPagedListAsync(query, pageNumber ?? 1, pageSize, token);
  public static async Task<PagedList<TKey>> ToPagedListAsync<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, int pageNumber, int? pageSize = null, CancellationToken token = default)
    => await ToPagedListAsync(query.Select(keySelector), pageNumber, pageSize, token);

}
