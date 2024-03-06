using System.Linq.Expressions;

namespace System.Linq;
public static class IQueryableExtensions {

  public static IQueryable<T> CreateQueryFromProvider<T>(this IQueryable queryable) => queryable.Provider.CreateQuery<T>(queryable);
  public static IQueryable<T> CreateQueryFromProvider<TSource, T>(this IQueryable<TSource> queryable) => queryable.Provider.CreateQuery<TSource, T>(queryable);

  public static bool IsOrderedQueryable<T>(this IQueryable<T> queryable) {
    if (queryable == null) {
      throw new ArgumentNullException(nameof(queryable));
    }
    return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
  }

  public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keySelector, bool isDescending)
    => isDescending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);

  //public static IOrderedQueryable<T> OrderByIf<T, TKey>(this IQueryable<T> source, bool condition, Expression<Func<T, TKey>> keySelector, IComparer<TKey> comparer) => condition ? source.OrderBy(keySelector, comparer) : source;
  //public static IOrderedQueryable<T> OrderByIf<T, TKey>(this IQueryable<T> source, bool condition, Expression<Func<T, TKey>> keySelector, bool isDescending) => condition ? source.OrderBy(keySelector, isDescending) : source;
  //public static IOrderedQueryable<T> OrderByDescendingIf<T, TKey>(this IQueryable<T> source, bool condition, Expression<Func<T, TKey>> keySelector, IComparer<TKey> comparer) => condition ? source.OrderByDescending(keySelector, comparer) : source;
  //public static IOrderedQueryable<T> OrderByDescendingIf<T, TKey>(this IQueryable<T> source, bool condition, Expression<Func<T, TKey>> keySelector) => condition ? source.OrderByDescending(keySelector) : source;

  public static IQueryable<T> Skip<T>(this IQueryable<T> query, int? skip) => skip.HasValue ? query.Skip(skip.Value) : query;
  public static IQueryable<T> Take<T>(this IQueryable<T> query, int? take) => take.HasValue ? query.Take(take.Value) : query;

  public static TSource? FirstOrDefaultDebug<TSource>(this IQueryable<TSource> source, StringBuilder? debugInfo = null) {
    debugInfo?.AppendLine($"{source}");
    return source.FirstOrDefault();
  }

  public static TSource? TryFirstOrDefault<TSource>(this IQueryable<TSource> source, StringBuilder? debugInfo = null) {
    try {
      debugInfo?.AppendLine($"{source}");
      return source.FirstOrDefault();
    } catch (Exception ex) {
      throw new Exception($"{ex.Message}\n\n{source}");
    }
  }

  public static List<TSource> ToListDebug<TSource>(this IQueryable<TSource> source, StringBuilder? debugInfo = null) {
    debugInfo?.AppendLine($"{source}");
    return source.ToList();
  }

  public static List<TSource> TryToList<TSource>(this IQueryable<TSource> source, StringBuilder? debugInfo = null) {
    try {
      debugInfo?.AppendLine($"{source}");
      return source.ToList();
    } catch (Exception ex) {
      throw new Exception($"{ex.Message}\n\n{source}");
    }
  }

  public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> truePredicate, Expression<Func<T, bool>> falsePredicate) => condition ? source.Where(truePredicate) : source.Where(falsePredicate);
  public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> truePredicate, Expression<Func<T, int, bool>> falsePredicate) => condition ? source.Where(truePredicate) : source.Where(falsePredicate);

  public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate) => condition ? source.Where(predicate) : source;
  public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> predicate) => condition ? source.Where(predicate) : source;

  public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> source, Expression<Func<T, bool>>? predicate) => predicate != null ? source.Where(predicate) : source;
  public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> source, Expression<Func<T, int, bool>>? predicate) => predicate != null ? source.Where(predicate) : source;

}
