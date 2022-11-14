using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal.MethodCallTranslators {
  public class AS400StringSubstringTranslator : IMethodCallTranslator {

    static readonly MethodInfo _methodInfo
      = typeof(string).GetRuntimeMethod(nameof(string.Substring), new[] {
        typeof(int),
        typeof(int)
      });

    public virtual Expression Translate(MethodCallExpression methodCallExpression)
      => _methodInfo.Equals(methodCallExpression.Method)
      ? new SqlFunctionExpression("SUBSTR", methodCallExpression.Type, new[] {
        methodCallExpression.Object,
        methodCallExpression.Arguments[0].NodeType == ExpressionType.Constant
        ? (Expression)Expression.Constant(
          (int)((ConstantExpression)methodCallExpression.Arguments[0]).Value + 1)
        : Expression.Add(
          methodCallExpression.Arguments[0],
          Expression.Constant(1)),
        methodCallExpression.Arguments[1]
      })
      : null;
  }
}
