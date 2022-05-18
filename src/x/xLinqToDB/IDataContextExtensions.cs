using System.Linq.Expressions;

namespace LinqToDB;
public static class IDataContextExtensions {
  public static IModificationHandler<T> GetModificationHandler<T>(this IDataContext dataContext, Expression<Func<T, bool>> wherePredicate) where T : class => new ModificationHandler<T>(dataContext, wherePredicate);

  /// <summary>
  /// Inserts record into table, identified by <typeparamref name="T"/> if does not exist, using values from <paramref name="obj"/> parameter.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="dataContext"></param>
  /// <param name="obj"></param>
  /// <param name="predicate"></param>
  /// <returns>Number of affected records.</returns>
  public static int InsertIfNotExists<T>(this IDataContext dataContext, T obj, Expression<Func<T, bool>> predicate) where T : class
    => dataContext.GetTable<T>().Where(predicate).InsertIfNotExists(dataContext, obj);

}