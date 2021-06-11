using LinqToDB.Expressions;
using System;
using System.Reflection;

namespace LinqToDB.Tests.Model {
  public class Functions {
    private readonly IDataContext _ctx;

    public Functions(IDataContext ctx) {
      _ctx = ctx;
    }

    [Sql.TableFunction(Name = "GetParentByID")]
    public ITable<Parent> GetParentByID(int? id) {
      var methodInfo = typeof(Functions).GetMethod("GetParentByID", new[] { typeof(int?) })!;

      return _ctx.GetTable<Parent>(this, methodInfo, id);
    }

    [Sql.TableExpression("{0} {1} WITH (TABLOCK)")]
    public ITable<T> WithTabLock<T>() where T : class {
      var methodInfo = typeof(Functions).GetMethod("WithTabLock")!.MakeGenericMethod(typeof(T));

      return _ctx.GetTable<T>(this, methodInfo);
    }

    [Sql.TableExpression("{0} {1} WITH (TABLOCK)")] static ITable<T> WithTabLock1<T>() => throw new InvalidOperationException();

    static readonly MethodInfo _methodInfo = MemberHelper.MethodOf(() => WithTabLock1<int>()).GetGenericMethodDefinition();

    [Sql.TableExpression("{0} {1} WITH (TABLOCK)")] public static ITable<T> WithTabLock1<T>(IDataContext ctx) where T : class => ctx.GetTable<T>(null, _methodInfo.MakeGenericMethod(typeof(T)));

  }
}
