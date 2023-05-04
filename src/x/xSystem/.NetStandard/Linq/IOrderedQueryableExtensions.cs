using System.Linq.Expressions;

namespace System.Linq;
public static class IOrderedQueryableExtensions {

  public static IOrderedQueryable<T> ThenBy<T, TKey>(this IOrderedQueryable<T> source, Expression<Func<T, TKey>> keySelector, bool isDescending)
    => isDescending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);

}