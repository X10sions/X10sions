using System.Reflection;

namespace System.Linq.Expressions;
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

  private static readonly MethodInfo[] methods = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static);

  private static readonly string[] queryableOrderMethods =  {
        nameof(Queryable.OrderBy),
        nameof(Queryable.ThenBy),
        nameof(Queryable.OrderByDescending),
        nameof(Queryable.ThenByDescending)
    };

  public static bool IsExpressionBodyConstant<T>(this Expression<Func<T, bool>> expr) => expr.Body.NodeType == ExpressionType.Constant;

  public static bool IsOrderingMethod(this Expression expression) => queryableOrderMethods.Any(method => IsQueryableMethod(expression, method));

  public static bool IsQueryableMethod(this Expression expression, string method) => methods.Where(m => m.Name == method).Contains(GetQueryableMethod(expression));

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

  public static MemberExpression AsMemberExpression(this Expression expression) => expression switch {
    //null => null,
    MemberExpression me => me,
    UnaryExpression ue => ue.Operand.AsMemberExpression(),
    _ => throw new Exception()
  };

  public static MemberInfo GetBodyMemberInfo<T>(this Expression<Func<T, object>> expression) => expression.Body.GetMemberInfo();
  public static MemberInfo GetBodyMemberInfo<T>(this T instance, Expression<Action<T>> expression) => expression.Body.GetMemberInfo();
  public static List<MemberInfo> GetBodyMemberInfoList<T>(this T instance, params Expression<Func<T, object>>[] expressions) => expressions.Select(e => e.Body.GetMemberInfo()).ToList();
  public static string GetBodyMemberName<T>(this Expression<Func<T, object>> expression) => expression.Body.GetMemberInfo().Name;
  public static string GetBodyMemberName<T>(this T instance, Expression<Action<T>> expression) => expression.Body.GetMemberInfo().Name;
  public static List<string> GetBodyMemberNameList<T>(this T instance, params Expression<Func<T, object>>[] expressions) => expressions.Select(e => e.Body.GetMemberInfo().Name).ToList();
  public static MemberExpression GetMemberExpression<T, TProp>(this Expression<Func<T, TProp>> member) => member.Body.AsMemberExpression();
  public static MemberInfo GetMemberInfo<T, TProp>(this Expression<Func<T, TProp>> exp) => exp.GetMemberExpression().Member;
  public static MemberInfo GetMemberInfo(this Expression expression) {
    if (expression == null)
      throw new ArgumentException(nameof(expression));
    return expression switch {
      LambdaExpression le => le.Body.GetMemberInfo(),
      MemberExpression me => me.Member,
      MethodCallExpression mce => mce.Method,
      NewExpression ne => ne.Constructor,
      UnaryExpression ue => ue.Operand.GetMemberInfo(),
      _ => throw new ArgumentException($"Invalid Expression Type: '{expression.Type}' NodeType: '{expression.NodeType}'")
    };
  }
  public static string GetMemberName<T, TProp>(this Expression<Func<T, TProp>> exp) => exp.GetMemberExpression().Member.Name;
  public static string GetMemberName(this Expression expression) => expression.NodeType switch {
    ExpressionType.MemberAccess => ((MemberExpression)expression).Member.Name,
    ExpressionType.Convert => GetMemberName(((UnaryExpression)expression).Operand),
    _ => throw new NotSupportedException(expression.NodeType.ToString())
  };
  public static MethodInfo? GetSetMethod<T>(this Expression<Func<T>> expression) => ((expression?.Body as MemberExpression)?.Member as PropertyInfo)?.GetSetMethod(true);

  public static void SetValue<T, TValue>(this Expression<Func<T>> getSetExpression, T instance, TValue value) => GetSetMethod(getSetExpression)?.Invoke(instance, new object[] { value });

  #endregion "GetMemberNames"

  #region "Predicates"

  public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) {
    if (expr1.Equals(expr2)) return expr1;
    if (expr1 == null || expr1.Equals(True<T>())) return expr2;
    if (expr2 == null || expr2.Equals(True<T>())) return expr1;
    if (new[] { expr1, expr2 }.Contains(False<T>())) return False<T>();
    var replace = expr1.ReplaceExpressions(expr2);
    return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(replace.left, replace.right), replace.parameter);
  }

  public static Expression<Func<T, bool>> AndAlsoIf<T>(this Expression<Func<T, bool>> expr1, bool? ifTrue, Expression<Func<T, bool>> truePredicate) => ifTrue.HasValue && ifTrue.Value ? expr1.AndAlso(truePredicate) : expr1;
  public static Expression<Func<T, bool>> AndAlsoIf<T>(this Expression<Func<T, bool>> expr1, bool? ifTrue, Expression<Func<T, bool>> truePredicate, Expression<Func<T, bool>> falsePredicate) => expr1.AndAlso(ifTrue.HasValue && ifTrue.Value ? truePredicate : falsePredicate);
  public static Expression<Func<T, bool>> Compare<T, TProp>(this Expression<Func<T, TProp>> field, TProp value, ExpressionType expressionType) {
    var valueConstantExpression = Expression.Constant(value, typeof(TProp));
    var binaryExpression = expressionType switch {
      ExpressionType.Equal => Expression.Equal(field.Body, valueConstantExpression),
      ExpressionType.GreaterThan => Expression.GreaterThan(field.Body, valueConstantExpression),
      ExpressionType.GreaterThanOrEqual => Expression.GreaterThanOrEqual(field.Body, valueConstantExpression),
      ExpressionType.LessThan => Expression.LessThan(field.Body, valueConstantExpression),
      ExpressionType.LessThanOrEqual => Expression.LessThanOrEqual(field.Body, valueConstantExpression),
      ExpressionType.NotEqual => Expression.NotEqual(field.Body, valueConstantExpression),
      _ => throw new NotImplementedException(expressionType.ToString())
    };
    return Expression.Lambda<Func<T, bool>>(binaryExpression, field.Parameters);
  }

  public static Expression<Func<T, bool>> Equal<T, TProp>(this Expression<Func<T, TProp>> field, TProp value) => Expression.Lambda<Func<T, bool>>(Expression.Equal(field.Body, Expression.Constant(value, typeof(TProp))), field.Parameters);
  //public static Expression<Func<T, bool>> Equal<T, TProp>(this Expression<Func<T, TProp?>> field, TProp? value) where TProp : struct => Expression.Lambda<Func<T, bool>>(Expression.Equal(field.Body, Expression.Convert(Expression.Constant(value), typeof(TProp?))), field.Parameters);
  public static Expression<Func<T, bool>> Equal<T, TProp>(this Expression<Func<T, TProp?>> field, TProp value) where TProp : struct => Expression.Lambda<Func<T, bool>>(Expression.Equal(field.Body, Expression.Constant(value, typeof(TProp?))), field.Parameters);
  public static Expression<Func<T, bool>> Equal<T, TProp>(this Expression<Func<T, TProp?>> field, TProp? value) where TProp : struct => Expression.Lambda<Func<T, bool>>(Expression.Equal(field.Body, Expression.Constant(value, typeof(TProp?))), field.Parameters);
  public static Expression<Func<T, bool>> EqualNull<T, TProp>(this Expression<Func<T, TProp?>> field) where TProp : struct => Expression.Lambda<Func<T, bool>>(Expression.Equal(field.Body, Expression.Constant(null, typeof(TProp?))), field.Parameters);
  public static Expression<Func<T, bool>> False<T>() => x => false;
  public static Expression<Func<T, bool>> GreaterThan<T, TProp>(this Expression<Func<T, TProp>> field, TProp value) =>  Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(field.Body, Expression.Constant(value, typeof(TProp))), field.Parameters);
  public static Expression<Func<T, bool>> GreaterThanOrEqual<T, TProp>(this Expression<Func<T, TProp>> field, TProp value) => Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(field.Body, Expression.Constant(value, typeof(TProp))), field.Parameters);
  public static Expression<Func<T, bool>> LessThan<T, TProp>(this Expression<Func<T, TProp>> field, TProp value) => Expression.Lambda<Func<T, bool>>(Expression.LessThan(field.Body, Expression.Constant(value, typeof(TProp))), field.Parameters);
  public static Expression<Func<T, bool>> LessThanOrEqual<T, TProp>(this Expression<Func<T, TProp>> field, TProp value) => Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(field.Body, Expression.Constant(value, typeof(TProp))), field.Parameters);
  public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression) => Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters);
  public static Expression<Func<T, bool>> NotEqual<T, TProp>(this Expression<Func<T, TProp>> field, TProp value) => Expression.Lambda<Func<T, bool>>(Expression.NotEqual(field.Body, Expression.Constant(value, typeof(TProp))), field.Parameters);
  public static Expression<Func<T, bool>> NotEqualNull<T, TProp>(this Expression<Func<T, TProp?>> field) where TProp : struct => Expression.Lambda<Func<T, bool>>(Expression.NotEqual(field.Body, Expression.Constant(null, typeof(TProp?))), field.Parameters);
  public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) {
    if (expr1.Equals(expr2)) return expr1;
    if (expr1 == null || expr1.Equals(False<T>())) return expr2;
    if (expr2 == null || expr2.Equals(False<T>())) return expr1;
    if (new[] { expr1, expr2 }.Contains(True<T>())) return True<T>();
    var replace = expr1.ReplaceExpressions(expr2);
    return Expression.Lambda<Func<T, bool>>(Expression.OrElse(replace.left, replace.right), replace.parameter);
  }

  public static Expression<Func<T, bool>> OrElseIf<T>(this Expression<Func<T, bool>> expr1, bool? ifTrue, Expression<Func<T, bool>> truePredicate) => ifTrue.HasValue && ifTrue.Value ? expr1.OrElse(truePredicate) : expr1;
  public static Expression<Func<T, bool>> OrElseIf<T>(this Expression<Func<T, bool>> expr1, bool? ifTrue, Expression<Func<T, bool>> truePredicate, Expression<Func<T, bool>> falsePredicate) => expr1.OrElse(ifTrue.HasValue && ifTrue.Value ? truePredicate : falsePredicate);

  public static Expression<Func<T, bool>> Predicate<T>(bool value) => x => value;
  public static Expression<Func<T, bool>> Predicate<T>(this Expression<Func<T, bool>> predicate) => predicate;
  public static Expression<Func<T, bool>> True<T>() => x => true;
  #endregion "Predicates"

  public static Expression Replace(this Expression expression, Expression searchExpression, Expression replaceExpression) => new ReplaceExpressionVisitor(searchExpression, replaceExpression).Visit(expression);

  public static (Expression left, Expression right, ParameterExpression parameter) ReplaceExpressions<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) {
    var parameter = Expression.Parameter(typeof(T));
    return (new ReplaceExpressionVisitor(expr1.Parameters[0], parameter).Visit(expr1.Body), new ReplaceExpressionVisitor(expr2.Parameters[0], parameter).Visit(expr2.Body), parameter);
  }

  public static Expression<Func<TNewParam, TResult>> ReplaceParameter<TNewParam, TOldParam, TResult>(this Expression<Func<TOldParam, TResult>> expression) where TNewParam : TOldParam {
    var param = Expression.Parameter(typeof(TNewParam));
    return Expression.Lambda<Func<TNewParam, TResult>>(expression.Body.Replace(expression.Parameters[0], param), param);
  }

  // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters

  public static Expression<Func<T2, T1, TResult>> SwapParameters00<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> exprFunc) where T1 : class where T2 : class => Expression.Lambda<Func<T2, T1, TResult>>(exprFunc.Body, exprFunc.Parameters[1], exprFunc.Parameters[0]);
  public static Expression<Func<T2, T1, TResult>> SwapParameters01<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> exprFunc) where T1 : class where T2 : class? => Expression.Lambda<Func<T2, T1, TResult>>(exprFunc.Body, exprFunc.Parameters[1], exprFunc.Parameters[0]);
  public static Expression<Func<T2, T1, TResult>> SwapParameters10<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> exprFunc) where T1 : class? where T2 : class => Expression.Lambda<Func<T2, T1, TResult>>(exprFunc.Body, exprFunc.Parameters[1], exprFunc.Parameters[0]);
  public static Expression<Func<T2, T1, TResult>> SwapParameters11<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> exprFunc) where T1 : class? where T2 : class? => Expression.Lambda<Func<T2, T1, TResult>>(exprFunc.Body, exprFunc.Parameters[1], exprFunc.Parameters[0]);

  public static Expression<Func<T2, T1, TResult>> SwapParameters1<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> exprFunc)
    => Expression.Lambda<Func<T2, T1, TResult>>(exprFunc.Body, exprFunc.Parameters[1], exprFunc.Parameters[0]);

  public static Expression<Func<T2, T1, TResult>> SwapParameters2<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> exprFunc)
    => (t1, t2) => exprFunc.Compile()(t2, t1);

  public static Expression<Func<T2, T1, TResult>> SwapParameters3<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> exprFunc) {
    var param1 = Expression.Parameter(typeof(T1));
    var param2 = Expression.Parameter(typeof(T2));
    return Expression.Lambda<Func<T2, T1, TResult>>(exprFunc.Body.Replace(exprFunc.Parameters[0], param1).Replace(exprFunc.Parameters[1], param2), param1, param2);
  }

  public static Expression<Func<T, TResult>> Unbox<T, TResult>(this Expression<Func<T, object>> original)
    => Expression.Lambda<Func<T, TResult>>(Expression.Convert(original.Body, typeof(TResult)), original.Parameters);

}