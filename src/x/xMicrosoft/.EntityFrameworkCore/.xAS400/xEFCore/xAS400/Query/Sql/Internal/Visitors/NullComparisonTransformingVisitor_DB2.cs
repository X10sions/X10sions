using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Remotion.Linq.Parsing;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace xEFCore.xAS400.Query.Sql.Internal.Visitors {
  class NullComparisonTransformingVisitor_DB2 : RelinqExpressionVisitor {

    public NullComparisonTransformingVisitor_DB2(IReadOnlyDictionary<string, object> paramValues) {
      _paramValues = paramValues;
    }

    readonly IReadOnlyDictionary<string, object> _paramValues;

    protected override Expression VisitBinary(BinaryExpression node) {
      if (node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual) {
        Expression expression2 = node.Left.RemoveConvert();
        Expression expression3 = node.Right.RemoveConvert();
        ParameterExpression parameterExpression = (expression2 as ParameterExpression) ?? (expression3 as ParameterExpression);
        if (parameterExpression != null && _paramValues.TryGetValue(parameterExpression.Name, out object obj)) {
          Expression expression4 = (expression2 is ParameterExpression) ? expression3 : expression2;
          ConstantExpression constantExpression;
          if ((constantExpression = (expression4 as ConstantExpression)) != null) {
            if (obj == null && constantExpression.Value == null) {
              if (node.NodeType != ExpressionType.Equal) {
                return Expression.Constant(false);
              }
              return Expression.Constant(true);
            }
            if (obj == null && constantExpression.Value != null) {
              goto IL_00be;
            }
            if (obj != null && constantExpression.Value == null) {
              goto IL_00be;
            }
          }
          if (obj == null) {
            if (node.NodeType != ExpressionType.Equal) {
              return Expression.Not(new IsNullExpression(expression4));
            }
            return new IsNullExpression(expression4);
          }
        }
      }
      return base.VisitBinary(node);
      IL_00be:
      if (node.NodeType != ExpressionType.Equal) {
        return Expression.Constant(true);
      }
      return Expression.Constant(false);
    }
  }

}