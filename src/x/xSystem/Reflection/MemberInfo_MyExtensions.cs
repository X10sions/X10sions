using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Reflection {
  [Obsolete("Try get rid of these: used by CommonORM")]
  public static class MemberInfo_MyExtensions {

    //makes expression for specific prop
    public static Expression<Func<TSource, object>> MyGetExpression<TSource>(string propertyName) {
      var pe = Expression.Parameter(typeof(TSource), "x");
      return Expression.Lambda<Func<TSource, object>>(Expression.Convert(Expression.PropertyOrField(pe, propertyName), typeof(object)), pe);
    }

    public static Expression<Func<TSource, object>> MyGetExpression<TSource>(this MemberInfo prop) => MyGetExpression<TSource>(prop.Name);

    public static Func<TSource, object> MyGetFunc<TSource>(string propertyName) => MyGetExpression<TSource>(propertyName).Compile();  //only need compiled expression

    public static Func<TSource, object> MyGetFunc<TSource>(this MemberInfo prop) => MyGetFunc<TSource>(prop.Name);

    public static IOrderedEnumerable<TSource> MyOrderBy<TSource>(this IEnumerable<TSource> source, string propertyName) => source.OrderBy(MyGetFunc<TSource>(propertyName));

    //OrderBy overload
    public static IOrderedQueryable<TSource> MyOrderBy<TSource>(this IQueryable<TSource> source, string propertyName) => source.OrderBy(MyGetExpression<TSource>(propertyName));

  }
}