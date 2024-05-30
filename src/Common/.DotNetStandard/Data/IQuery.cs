using Common.Collections.Paged;
using System.Linq.Expressions;

namespace Common.Data;

public interface IQuery<T> where T : class {
  IQueryable<T> Queryable { get; }
  Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
  Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
  Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
  Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
}

public static class IQueryExtensions {
  public static IQuery<TResult> Select<T, TResult>(this IQuery<T> qry, Expression<Func<T, TResult>> selector) where T : class where TResult : class => qry.Select(selector);
  public static IQuery<T> Skip<T>(this IQuery<T> qry, int? skip = null) where T : class => skip.HasValue ? qry.Skip(skip.Value) : qry;
  public static IQuery<T> Take<T>(this IQuery<T> qry, int? take = null) where T : class => take.HasValue ? qry.Take(take.Value) : qry;
  public static IQuery<T> Where<T>(this IQuery<T> qry, Expression<Func<T, bool>> prediacte) where T : class => qry.Where(prediacte);


  public async static Task<bool> AnyAsync<T>(this IQuery<T> qry, CancellationToken token = default) where T : class => await qry.AnyAsync(x => true, token);
  public static async Task<int> CountAsync<T>(this IQuery<T> qry, CancellationToken token = default) where T : class => await qry.CountAsync(x => true, token);
  public static async Task<T?> FirstOrDefaultAsync<T>(this IQuery<T> qry, CancellationToken token = default) where T : class => await qry.FirstOrDefaultAsync(x => true, token);
  public static async Task<List<T>> ToListAsync<T>(this IQuery<T> qry, CancellationToken token = default) where T : class => await qry.ToListAsync(x => true, token);

  public async static Task<PagedList<T>> ToPagedListAsync<T>(this IQuery<T> qry, int pageNumber = 1, int? pageSize = 10, CancellationToken token = default) where T : class {
    var options = new PagedListOptions(pageNumber, pageSize, await qry.CountAsync(token));
    var items = await qry.Skip(options.GetSkip()).Take(options.GetTake()).ToListAsync(token);
    return new PagedList<T>(items, options);
  }

}