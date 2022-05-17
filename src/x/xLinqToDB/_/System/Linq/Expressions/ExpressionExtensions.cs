namespace System.Linq.Expressions;
public static class ExpressionExtensions {

  public static Expression<Func<T, bool>> ToPredicateExpression<T, TValue>(this Expression<Func<T, TValue>> expression, TValue value) {
    var param1 = expression.Parameters.First();
    var predicateExpr = Expression.Lambda<Func<T, bool>>(Expression.Equal(expression.Body, Expression.Constant(value)), new[] { param1 });
    return predicateExpr;
  }

}