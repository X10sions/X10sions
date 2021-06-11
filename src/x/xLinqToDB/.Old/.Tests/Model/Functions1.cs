using LinqToDB.Expressions;
using System;
using System.Reflection;

namespace LinqToDB.Tests.Model {
  public static class Functions1 {
    [Sql.TableExpression("{0} {1} WITH (TABLOCK)")] static ITable<T> WithTabLock<T>() => throw new InvalidOperationException();

    static readonly MethodInfo _methodInfo = MemberHelper.MethodOf(() => WithTabLock<int>()).GetGenericMethodDefinition();

    [Sql.TableExpression("{0} {1} WITH (TABLOCK)")] public static ITable<T> WithTabLock<T>(this IDataContext ctx) where T : class => ctx.GetTable<T>(null, _methodInfo.MakeGenericMethod(typeof(T)));
  }
}
