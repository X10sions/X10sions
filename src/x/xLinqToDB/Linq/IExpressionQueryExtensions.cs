using LinqToDB.Internal.Linq;

namespace LinqToDB.Linq;

public static class IExpressionQueryExtensions {
  public static ITable<T> GetTable<T>(this IExpressionQuery table) where T : class => table.DataContext.GetTable<T>();
  //public static ITable<T> GetTable<TSource, T>(this IExpressionQuery<T> table) where T : class => table.DataContext.GetTable<T>();
}
