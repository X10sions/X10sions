using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Expressions {

  public static class ExpressionExtensions {

    #region "LinqToDB.Linq.Expressions"

    public static LambdaExpression AsLambdaExpression<TR>(this Expression<Func<TR>> func) => func;
    public static LambdaExpression AsLambdaExpression<T1, TR>(this Expression<Func<T1, TR>> func) => func;
    public static LambdaExpression AsLambdaExpression<T1, T2, T3, TR>(this Expression<Func<T1, T2, T3, TR>> func) => func;
    public static LambdaExpression AsLambdaExpression<T1, T2, T3, T4, TR>(this Expression<Func<T1, T2, T3, T4, TR>> func) => func;
    public static LambdaExpression AsLambdaExpression<T1, T2, T3, T4, T5, TR>(this Expression<Func<T1, T2, T3, T4, T5, TR>> func) => func;
    public static LambdaExpression AsLambdaExpression<T1, T2, T3, T4, T5, T6, TR>(this Expression<Func<T1, T2, T3, T4, T5, T6, TR>> func) => func;
    public static LambdaExpression AsLambdaExpressionL<T1, T2, TR>(this Expression<Func<T1, T2, TR>> func) => func;

    public static ConstructorInfo ConstructorOf<T>(this Expression<Func<T, object>> func) => (ConstructorInfo)GetMemberInfo(func);
    public static ConstructorInfo ConstructorOf(this Expression<Func<object>> func) => (ConstructorInfo)GetMemberInfo(func);
    public static FieldInfo FieldOf<T>(this Expression<Func<T, object>> func) => (FieldInfo)GetMemberInfo(func);

    public static MethodInfo MethodOf<T>(this Expression<Func<T, object>> func) {
      var memberInfo = func.GetMemberInfo();
      return (memberInfo is PropertyInfo pi) ? pi.GetGetMethod() : (MethodInfo)memberInfo;
    }

    public static MethodInfo MethodOf(this Expression<Func<object>> func) {
      var memberInfo = func.GetMemberInfo();
      return (memberInfo is PropertyInfo pi) ? pi.GetGetMethod() : (MethodInfo)memberInfo;
    }

    public static PropertyInfo PropertyOf<T>(this Expression<Func<T, object>> func) => (PropertyInfo)GetMemberInfo(func);

    #endregion "LinqToDB.Linq.Expressions"

    private static readonly MethodInfo[] _methods = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static);

    private static readonly string[] _queryableOrderMethods =  {
        nameof(Queryable.OrderBy),
        nameof(Queryable.ThenBy),
        nameof(Queryable.OrderByDescending),
        nameof(Queryable.ThenByDescending)
    };

    public static Expression<Func<TSource, TDestination>> Combine<TSource, TDestination>(this IEnumerable<Expression<Func<TSource, TDestination>>> selectors) => selectors.ToArray().Combine();

    public static Expression<Func<TSource, TDestination>> Combine<TSource, TDestination>(
      this Expression<Func<TSource, TDestination>> firstSelector,
      params Expression<Func<TSource, TDestination>>[] OtherSelectors
      ) => (new[] { firstSelector }).Union(OtherSelectors).Combine();

    public static Expression<Func<T, bool>> False<T>() => x => false;

    public static bool IsExpressionBodyConstant<T>(this Expression<Func<T, bool>> expr) => expr.Body.NodeType == ExpressionType.Constant;

    public static bool IsOrderingMethod(this Expression expression) => _queryableOrderMethods.Any(method => IsQueryableMethod(expression, method));

    public static bool IsQueryableMethod(this Expression expression, string method) => _methods.Where(m => m.Name == method).Contains(GetQueryableMethod(expression));

    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression) => Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters);

    public static Expression<Func<T, bool>> Predicate<T>(bool value) => x => value;

    public static Expression<Func<T, bool>> Predicate<T>(this Expression<Func<T, bool>> predicate) => predicate;

    public static Expression<Func<T, bool>> True<T>() => x => true;

    private static MethodInfo? GetQueryableMethod(this Expression expression) {
      if (expression.NodeType == ExpressionType.Call) {
        var call = (MethodCallExpression)expression;
        if (call.Method.IsStatic && call.Method.DeclaringType == typeof(Queryable)) {
          return call.Method.GetGenericMethodDefinition();
        }
      }
      return null;
    }

    #region "GetMemberNames"

    public static MemberInfo GetBodyMemberInfo<T>(this Expression<Func<T, object>> expression) => expression.Body.GetMemberInfo();

    public static MemberInfo GetBodyMemberInfo<T>(this T instance, Expression<Action<T>> expression)
      => expression.Body.GetMemberInfo();

    public static List<MemberInfo> GetBodyMemberInfoList<T>(this T instance, params Expression<Func<T, object>>[] expressions) {
      var list = new List<MemberInfo>();
      foreach (var e in expressions) {
        list.Add(e.Body.GetMemberInfo());
      }
      return list;
    }

    public static string GetBodyMemberName<T>(this Expression<Func<T, object>> expression) => expression.Body.GetMemberInfo().Name;

    public static string GetBodyMemberName<T>(this T instance, Expression<Action<T>> expression) => expression.Body.GetMemberInfo().Name;

    public static List<string> GetBodyMemberNameList<T>(this T instance, params Expression<Func<T, object>>[] expressions) {
      var list = new List<string>();
      foreach (var e in expressions) {
        list.Add(e.Body.GetMemberInfo().Name);
      }
      return list;
    }

    public static MemberInfo GetMemberInfo(this Expression expression) {
      if (expression == null) throw new ArgumentException(nameof(expression));
      return expression switch {
        LambdaExpression le => le.Body.GetMemberInfo(),
        MemberExpression me => me.Member,
        MethodCallExpression mce => mce.Method,
        NewExpression ne => ne.Constructor,
        UnaryExpression ue => ue.Operand.GetMemberInfo(),
        _ => throw new ArgumentException($"Invalid Expression Type: '{expression.Type}' NodeType: '{expression.NodeType}'")
      };
    }

    #endregion "GetMemberNames"

  }
}