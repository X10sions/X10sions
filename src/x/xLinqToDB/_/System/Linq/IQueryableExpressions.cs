using LinqToDB;
using LinqToDB.Linq;

namespace System.Linq;
public static class IQueryableExpressions {

  public static T FirstOrInsert<T>(this IQueryable<T> queryable, IValueInsertable<T> insertable) {
    Console.WriteLine(queryable);
    var t = queryable.FirstOrDefault();
    if (t == null) {
      Console.WriteLine(insertable);
      insertable.Insert();
      Console.WriteLine(queryable);
      t = queryable.FirstOrDefault();
    }
    return t;
  }

}