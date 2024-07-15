using System.Linq.Expressions;
using System.Reflection;
using Common;
using DotNetBrightener.LinQToSqlBuilder.Resolver.ExpressionTree;
using DotNetBrightener.LinQToSqlBuilder.ValueObjects;

namespace DotNetBrightener.LinQToSqlBuilder.Resolver;

/// <summary>
/// Provides methods to perform resolution to SQL expressions from given lambda expressions
/// </summary>
partial class LambdaResolver {
  public void ResolveQuery<T>(Expression<Func<T, bool>> expression) {
    var expressionTree = ResolveQuery(expression.Body);
    BuildSql(expressionTree);
  }

  private Node ResolveQuery(ConstantExpression constantExpression) => new ValueNode { Value = constantExpression.Value };

  private Node ResolveQuery(UnaryExpression unaryExpression) => new SingleOperationNode {
    Operator = unaryExpression.NodeType,
    Child = ResolveQuery(unaryExpression.Operand)
  };

  private Node ResolveQuery(BinaryExpression binaryExpression) => new OperationNode {
    Left = ResolveQuery(binaryExpression.Left),
    Operator = binaryExpression.NodeType,
    Right = ResolveQuery(binaryExpression.Right)
  };

  private Node ResolveQuery(MethodCallExpression callExpression) {
    if (Enum.TryParse(callExpression.Method.Name, true, out LikeMethod callFunction)) {
      var member = callExpression.Object as MemberExpression;
      var fieldValue = (string)callExpression.Arguments.First().GetExpressionValue();
      return new LikeNode {
        MemberNode = new MemberNode {
          TableName = GetTableName(member),
          FieldName = GetColumnName(callExpression.Object)
        },
        Method = callFunction,
        Value = fieldValue
      };
    }

    var value = callExpression.ResolveMethodCall();
    return new ValueNode {
      Value = value
    };
  }

  private Node ResolveQuery(MemberExpression memberExpression, MemberExpression rootExpression = null) {
#if NETCOREAPP2_1
            if (rootExpression == null)
                rootExpression = memberExpression;
#else
    rootExpression ??= memberExpression;
#endif
    switch (memberExpression.Expression.NodeType) {
      case ExpressionType.Parameter:
        return new MemberNode {
          TableName = GetTableName(rootExpression),
          FieldName = GetColumnName(rootExpression)
        };

      case ExpressionType.MemberAccess:
        return ResolveQuery(memberExpression.Expression as MemberExpression, rootExpression);

      case ExpressionType.Call:
      case ExpressionType.Constant:
        return new ValueNode {
          Value = rootExpression.GetExpressionValue()
        };

      default:
        throw new ArgumentException("Expected member expression");
    }
  }


  public void QueryByIsIn<T>(Expression<Func<T, object>> expression, SqlBuilderBase sqlQuery) {
    var fieldName = GetColumnName(expression);
    Builder.QueryByIsIn(GetTableName<T>(), fieldName, sqlQuery);
  }

  public void QueryByIsIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values) {
    var fieldName = GetColumnName(expression);
    Builder.QueryByIsIn(GetTableName<T>(), fieldName, values);
  }

  public void QueryByNotIn<T>(Expression<Func<T, object>> expression, SqlBuilderBase sqlQuery) {
    var fieldName = GetColumnName(expression);
    Builder.Not();
    Builder.QueryByIsIn(GetTableName<T>(), fieldName, sqlQuery);
  }

  public void QueryByNotIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values) {
    var fieldName = GetColumnName(expression);
    Builder.Not();
    Builder.QueryByIsIn(GetTableName<T>(), fieldName, values);
  }


  #region Fail functions

  private Node ResolveQuery(Expression expression)
    => throw new ArgumentException(string.Format("The provided expression '{0}' is currently not supported", expression.NodeType));

  #endregion
}