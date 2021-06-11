using System.Linq.Expressions;

namespace System.Linq {
  public static class IOrderedQueryableExtensions {

    public static IOrderedQueryable<T> AddOrdering<T, TKey>(this IOrderedQueryable<T> source, Expression<Func<T, TKey>> keySelector, bool descending) {
      if (source.Expression.Type != typeof(IOrderedQueryable<T>)) {
        return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
      }
      return descending ? ((IOrderedQueryable<T>)source).ThenByDescending(keySelector) : ((IOrderedQueryable<T>)source).ThenBy(keySelector);
    }

  }
}