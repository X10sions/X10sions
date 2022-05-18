using LinqToDB;
using LinqToDB.Linq;

namespace System.Linq;
public static class IQueryableExpressions {

  public static T FirstOrInsert<T>(this IQueryable<T> queryable, IValueInsertable<T> insertable) {
    var t = queryable.FirstOrDefault();
    if (t == null) {
      insertable.Insert();
      t = queryable.FirstOrDefault();
    }
    return t;
  }

  /// <summary>
  /// Inserts record into table, identified by <typeparamref name="T"/> if does not exist, using values from <paramref name="obj"/> parameter.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="queryable"></param>
  /// <param name="dataContext"></param>
  /// <param name="obj"></param>
  /// <returns>Number of affected records.</returns>
  public static int InsertIfNotExists<T>(this IQueryable<T> queryable, IDataContext dataContext, T obj) where T : notnull {
    if (!queryable.Any()) {
      return dataContext.Insert(obj);
    }
    return 0;
  }

}