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

  //public static int InsertIfNotExists<T>(this IQueryable<T> queryable, IValueInsertable<T> insertable) => queryable.Any() ? 0 : insertable.Insert();

  /// <summary>
  /// Inserts record into table, identified by <typeparamref name="T"/> if does not exist, using values from <paramref name="obj"/> parameter.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="queryable"></param>
  /// <param name="dataContext"></param>
  /// <param name="obj"></param>
  /// <returns>Number of affected records.</returns>
  //public static int InsertIfNotExists<T>(this IQueryable<T> queryable, IDataContext dataContext, T obj) where T : notnull => queryable.Any() ? 0 : dataContext.Insert(obj);
  //public static T InsertWithOutputIfNotExists<T>(this IQueryable<T> queryable, IDataContext dataContext, T obj) where T : class => queryable.FirstOrDefault() ?? dataContext.GetTable<T>().InsertWithOutputIfNotExists(dataContext, obj);

}