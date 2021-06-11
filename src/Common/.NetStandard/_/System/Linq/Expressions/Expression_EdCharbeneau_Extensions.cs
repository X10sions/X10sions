using Common.Linq;

namespace System.Linq.Expressions {
  public static class Expression_EdCharbeneau_Extensions {

    // 2013-10-31: https://github.com/EdCharbeneau/PredicateExtensions/blob/master/PredicateExtensions/PredicateExtensions.cs
    // https://www.nuget.org/packages/PredicateExtensions/

    public static Expression<Func<T, bool>> And_PE<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) => CombineLambdas_PE(left, right, ExpressionType.AndAlso);
    public static Expression<Func<T, bool>> Or_PE<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) => CombineLambdas_PE(left, right, ExpressionType.OrElse);

    static Expression<Func<T, bool>> CombineLambdas_PE<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, ExpressionType expressionType) {
      if (left.IsExpressionBodyConstant()) return right;
      var p = left.Parameters[0];
      var visitor = new SubstituteParameterExpressionVisitor();
      visitor.Substitues[right.Parameters[0]] = p;
      Expression body = Expression.MakeBinary(expressionType, left.Body, visitor.Visit(right.Body));
      return Expression.Lambda<Func<T, bool>>(body, p);
    }

  }
}
