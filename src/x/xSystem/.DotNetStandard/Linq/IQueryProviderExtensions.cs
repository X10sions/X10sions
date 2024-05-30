using System.Linq.Expressions;

namespace System.Linq;
public static class IQueryProviderExtensions {

  public static IQueryable<T> CreateQuery<T>(this IQueryProvider queryProvider, IQueryable queryable)
    => queryProvider.CreateQuery<T>(queryable.GetType().GenericTypeArguments[0], queryable.Expression);

  public static IQueryable<T> CreateQuery<TSource, T>(this IQueryProvider queryProvider, IQueryable<TSource> queryable)
    => queryProvider.CreateQuery<T>(typeof(TSource), queryable.Expression);

  static IQueryable<T> CreateQuery<T>(this IQueryProvider queryProvider, Type sourceType, Expression sourceExpression) {
    var expr = new CreateQueryExpressionVisitor<T>(sourceType).Visit(sourceExpression);
    return queryProvider.CreateQuery<T>(expr);
  }

  class CreateQueryExpressionVisitor<TTarget> : ExpressionVisitor {
    Type sourceType;// = typeof(TSource);
    Type targetType = typeof(TTarget);

    public CreateQueryExpressionVisitor(Type sourceType) {
      this.sourceType = sourceType;
    }

    protected override Expression VisitParameter(ParameterExpression node) => node.Type == sourceType ? Expression.Parameter(targetType, node.Name) : node;

    protected override Expression VisitMethodCall(MethodCallExpression node) {
      //if (node.Object == null && node.Method.IsGenericMethod) {
      // Static generic method
      var arguments = Visit(node.Arguments);
      //var genericArguments = node.Method.GetGenericArguments();
      var newArguments = new[] { targetType };
      var method = node.Method.GetGenericMethodDefinition().MakeGenericMethod(targetType);
      return Expression.Call(method, arguments);
      //}
      //return base.VisitMethodCall(node);
    }

  }

}