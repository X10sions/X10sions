using System.Linq.Expressions;

namespace LinqToDB;

public static class ITableExtensions {
  public static IModificationHandler<T> GetModificationHandler<T>(this ITable<T> table, Expression<Func<T, bool>> wherePredicate) where T : class => new ModificationHandler<T>(table, wherePredicate);
}