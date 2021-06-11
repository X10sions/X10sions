using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Collections.Paged {
  public static class PagedListExtensions {

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> superset, int numberOfPages) {
      var take = Convert.ToInt32(Math.Ceiling(superset.Count() / (double)numberOfPages));
      var result = new List<IEnumerable<T>>();
      for (var i = 0; i < numberOfPages; i++) {
        var chunk = superset.Skip(i * take).Take(take).ToList();
        if (chunk.Any()) {
          result.Add(chunk);
        }
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

    public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset, int pageNumber, int pageSize) => new PagedList<T>(superset, pageNumber, pageSize);
    public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset) => new PagedList<T>(superset, 1, superset.Count());
    public static IPagedList<TResult> Select<TSource, TResult>(this IPagedList<TSource> source, Func<TSource, TResult> selector) => new PagedList<TResult>(source, ((IEnumerable<TSource>)source).Select(selector));
    public static IPagedList<T> ToPagedList<T, TKey>(this IQueryable<T> superset, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize) => new PagedList<T, TKey>(superset, keySelector, pageNumber, pageSize);
    public static IPagedList<T> ToPagedList<T, TKey>(this IEnumerable<T> superset, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize) => new PagedList<T, TKey>(superset.AsQueryable(), keySelector, pageNumber, pageSize);
    public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> superset) => await Task.Run(() => superset.ToList());
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize) {
      var subset = new List<T>();
      var totalCount = 0;
      if ((superset != null) && (superset.Any())) {
        totalCount = superset.Count();
        subset.AddRange(
            (pageNumber == 1)
                ? await superset.Skip(0).Take(pageSize).ToListAsync().ConfigureAwait(false)
                : await superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync().ConfigureAwait(false)
        );
      }
      return new StaticPagedList<T>(subset, pageNumber, pageSize, totalCount);
    }

    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> superset, int pageNumber, int pageSize) => await ToPagedListAsync(superset.AsQueryable(), pageNumber, pageSize);
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int? pageNumber, int pageSize) => await ToPagedListAsync(superset.AsQueryable(), pageNumber ?? 1, pageSize);

  }
}