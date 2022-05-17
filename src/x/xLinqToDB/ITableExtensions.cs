using LinqToDB.Linq;
using System.Linq.Expressions;

namespace LinqToDB;

public static class ITableExtensions {
  public static IModificationHandler<T> GetModificationHandler<T>(this ITable<T> table, Expression<Func<T, bool>> wherePredicate) where T : class => new ModificationHandler<T>(table, wherePredicate);
  //public static IFirstOrInsertBuilder<T> GetFirstOrInsertBuilder<T>(this ITable<T> table) where T : notnull => new FirstOrInsertBuilder<T>(table);

}