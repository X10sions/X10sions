using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Security.Claims;

namespace xSystem.NetStandard;
public static class ClaimsPrincipalExtensions {

  /// <summary> https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Extensions.Core/src/PrincipalExtensions.cs </summary>
  public static string? FindFirstValue(this ClaimsPrincipal principal, string claimType)
    => principal is null ? throw new ArgumentNullException(nameof(principal)) : principal.FindFirst(claimType)?.Value;

}

public static class ExpressionHelperExtensions {

  private static readonly ConcurrentDictionary<LambdaExpression, string> _expressionTextCache = new(LambdaExpressionComparer.Instance);

  /// <summary> https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.ViewFeatures/src/ModelExpressionProvider.cs#L37 </summary>
  public static string GetExpressionText<TModel, TValue>(this Expression<Func<TModel, TValue>> expression)
    => expression is null ? throw new ArgumentNullException(nameof(expression)) : ExpressionHelper.GetExpressionText(expression, _expressionTextCache);

}
