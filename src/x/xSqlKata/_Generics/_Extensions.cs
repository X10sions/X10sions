using System;
using System.Linq;
using System.Linq.Expressions;

namespace SqlKata {
  public static class _Extensions {

    public static string[] GetMemberNames<T>(this Expression<Func<T, object>> expression) {
      if (expression.Body is NewExpression newExpression) {
        return newExpression.Members.Select(x => x.Name).ToArray();
      } else {
        throw new ArgumentException("Unexpected expression type.");
      }
    }

    public static string[] GetMemberNames<T, T1>(this Expression<Func<T, T1, object>> expression) {
      if (expression.Body is NewExpression newExpression) {
        return newExpression.Members.Select(x => x.Name).ToArray();
      }

      if (expression.Body is UnaryExpression unaryExpression) {
        if (unaryExpression.Operand is BinaryExpression binaryExpression) {
          var left = binaryExpression.Left as MemberExpression;
          var right = binaryExpression.Right as MemberExpression;

          return new string[] { left.Member.Name, right.Member.Name };
        }

        throw new ArgumentException("Unexpected expression type.");
      } else {
        throw new ArgumentException("Unexpected expression type.");
      }
    }

    public static string GetMemberName<T>(this Expression<Func<T, object>> expression) => GetMemberName(expression.Body);

    private static string GetMemberName(Expression expression) {
      if (expression == null)
        throw new ArgumentException("The expression cannot be null.");
      if (expression is MemberExpression memberExpression) return memberExpression.Member.Name;
      if (expression is MethodCallExpression methodCallExpression) return methodCallExpression.Method.Name;
      if (expression is UnaryExpression unaryExpression) {
        if (unaryExpression.Operand is MethodCallExpression methodCallExp) {
          return methodCallExp.Method.Name;
        } else if (unaryExpression.Operand is BinaryExpression binaryExpression) {
          var left = binaryExpression.Left as MemberExpression;
          var right = binaryExpression.Right as ConstantExpression;
          return $"{left.Member.Name} {binaryExpression.NodeType.ToMethod()} {right.Value}";
        }
        return ((MemberExpression)unaryExpression.Operand).Member.Name;
      }
      throw new ArgumentException("Invalid expression");
    }

    public static string ToMethod(this ExpressionType nodeType, bool rightIsNull = false) => nodeType switch
    {
      ExpressionType.Add => "+",
      ExpressionType.And => "&",
      ExpressionType.AndAlso => "AND",
      ExpressionType.Divide => "/",
      ExpressionType.Equal => rightIsNull ? "IS" : "=",
      ExpressionType.ExclusiveOr => "^",
      ExpressionType.GreaterThan => ">",
      ExpressionType.GreaterThanOrEqual => ">=",
      ExpressionType.LessThan => "<",
      ExpressionType.LessThanOrEqual => "<=",
      ExpressionType.Modulo => "%",
      ExpressionType.Multiply => "*",
      ExpressionType.Negate => "-",
      ExpressionType.Not => "NOT",
      ExpressionType.NotEqual => "<>",
      ExpressionType.Or => "|",
      ExpressionType.OrElse => "OR",
      ExpressionType.Subtract => "-",
      _ => throw new Exception($"Unsupported node type: {nodeType}"),
    };
  }
}
