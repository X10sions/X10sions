/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System.Linq.Expressions;
using System.Reflection;
using Common;
using LambdaSqlBuilder.Resolver.ExpressionTree;
using LambdaSqlBuilder.ValueObjects;

namespace LambdaSqlBuilder.Resolver {
  partial class LambdaResolver {
    public void ResolveQuery<T>(Expression<Func<T, bool>> expression) {
      var expressionTree = ResolveQuery(expression.Body);
      BuildSql(expressionTree);
    }

    private Node ResolveQuery(ConstantExpression constantExpression) {
      return new ValueNode() { Value = constantExpression.Value };
    }

    private Node ResolveQuery(UnaryExpression unaryExpression) {
      return new SingleOperationNode() {
        Operator = unaryExpression.NodeType,
        Child = ResolveQuery(unaryExpression.Operand)
      };
    }

    private Node ResolveQuery(BinaryExpression binaryExpression) {
      return new OperationNode {
        Left = ResolveQuery(binaryExpression.Left),
        Operator = binaryExpression.NodeType,
        Right = ResolveQuery(binaryExpression.Right)
      };
    }

    private Node ResolveQuery(MethodCallExpression callExpression) {
      LikeMethod callFunction;
      if (Enum.TryParse(callExpression.Method.Name, true, out callFunction)) {
        var member = callExpression.Object as MemberExpression;
        var fieldValue = (string)callExpression.Arguments.First().GetExpressionValue();

        return new LikeNode() {
          MemberNode = new MemberNode() {
            TableName = GetTableName(member),
            FieldName = GetColumnName(callExpression.Object)
          },
          Method = callFunction,
          Value = fieldValue
        };
      } else {
        var value = callExpression.ResolveMethodCall();
        return new ValueNode() { Value = value };
      }
    }

    private Node ResolveQuery(MemberExpression memberExpression, MemberExpression rootExpression = null) {
      rootExpression = rootExpression ?? memberExpression;
      switch (memberExpression.Expression.NodeType) {
        case ExpressionType.Parameter:
          return new MemberNode() { TableName = GetTableName(rootExpression), FieldName = GetColumnName(rootExpression) };
        case ExpressionType.MemberAccess:
          return ResolveQuery(memberExpression.Expression as MemberExpression, rootExpression);
        case ExpressionType.Call:
        case ExpressionType.Constant:
          return new ValueNode() { Value = rootExpression.GetExpressionValue() };
        default:
          throw new ArgumentException("Expected member expression");
      }
    }

    #region Helpers



    #endregion

    #region Fail functions

    private Node ResolveQuery(Expression expression) {
      throw new ArgumentException(string.Format("The provided expression '{0}' is currently not supported", expression.NodeType));
    }

    #endregion
  }
}
