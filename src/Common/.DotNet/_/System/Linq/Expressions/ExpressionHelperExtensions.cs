using Common.Linq.Expressions;
using System.Collections.Concurrent;

namespace System.Linq.Expressions;

public static class ExpressionHelperExtensions {

  private static readonly ConcurrentDictionary<LambdaExpression, string> _expressionTextCache = new(LambdaExpressionComparer.Instance);

  /// <summary> https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.ViewFeatures/src/ModelExpressionProvider.cs#L37 </summary>
  public static string GetExpressionText<TModel, TValue>(this Expression<Func<TModel, TValue>> expression)
    => expression is null ? throw new ArgumentNullException(nameof(expression)) : ExpressionHelper.GetExpressionText(expression, _expressionTextCache);

}