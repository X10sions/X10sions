using System.Linq.Expressions;

namespace System.Linq {
  public static class IQueryableExtensions {

    public static bool IsOrderedQueryable<T>(this IQueryable<T> queryable) {
      if (queryable == null) {
        throw new ArgumentNullException(nameof(queryable));
      }
      return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
    }

    public static IQueryable<T> SkipTakeToPage<T>(this IQueryable<T> source, int currentPage, int pageSize)
      => source.Skip((currentPage - 1) * pageSize).Take(pageSize);


    public static IOrderedQueryable<T> AddOrdering<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keySelector, bool descending) {
      if (source.Expression.Type != typeof(IOrderedQueryable<T>)) {
        return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
      }
      return descending ? ((IOrderedQueryable<T>)source).ThenByDescending(keySelector) : ((IOrderedQueryable<T>)source).ThenBy(keySelector);
    }

    //public static IOrderedQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) => condition ? source.OrderBy(keySelector, comparer) : source;
    //public static IOrderedQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector) => condition ? source.OrderBy(keySelector) : source;
    //public static IOrderedQueryable<TSource> OrderByDescendingIf<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) => condition ? source.OrderByDescending(keySelector, comparer) : source;
    //public static IOrderedQueryable<TSource> OrderByDescendingIf<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> keySelector) => condition ? source.OrderByDescending(keySelector) : source;

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> truePredicate, Expression<Func<TSource, bool>> falsePredicate) => condition ? source.Where(truePredicate) : source.Where(falsePredicate);
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, int, bool>> truePredicate, Expression<Func<TSource, int, bool>> falsePredicate) => condition ? source.Where(truePredicate) : source.Where(falsePredicate);

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate) => condition ? source.Where(predicate) : source;
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, int, bool>> predicate) => condition ? source.Where(predicate) : source;


  }
}