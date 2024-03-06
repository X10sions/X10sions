using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Remotion.Linq.Parsing;
using System.Linq.Expressions;

namespace xEFCore.xAS400.Query.Sql.Internal.Visitors {
  class BooleanExpressionTranslatingVisitor_DB2 : RelinqExpressionVisitor {
    bool _isSearchCondition;

    public Expression Translate(Expression expression, bool searchCondition) {
      _isSearchCondition = searchCondition;
      return (this).Visit(expression);
    }

    protected override Expression VisitBinary(BinaryExpression node) {
      bool isSearchCondition = _isSearchCondition;
      ExpressionType nodeType = node.NodeType;
      if (nodeType == ExpressionType.AndAlso || nodeType == ExpressionType.OrElse) {
        _isSearchCondition = true;
      } else {
        _isSearchCondition = false;
      }
      Expression left = Visit(node.Left);
      Expression right = Visit(node.Right);
      _isSearchCondition = isSearchCondition;
      node = node.Update(left, node.Conversion, right);
      return ApplyConversion(node);
    }

    protected override Expression VisitConditional(ConditionalExpression node) {
      bool isSearchCondition = _isSearchCondition;
      _isSearchCondition = true;
      Expression test = Visit(node.Test);
      _isSearchCondition = false;
      Expression ifTrue = Visit(node.IfTrue);
      Expression ifFalse = Visit(node.IfFalse);
      _isSearchCondition = isSearchCondition;
      node = node.Update(test, ifTrue, ifFalse);
      return ApplyConversion(node);
    }

    protected override Expression VisitConstant(ConstantExpression node) {
      return ApplyConversion(node);
    }

    protected override Expression VisitUnary(UnaryExpression node) {
      if (node.NodeType == ExpressionType.Not && node.Operand.IsSimpleExpression()) {
        return Visit(BuildCompareToExpression(node.Operand, false));
      }
      bool isSearchCondition = _isSearchCondition;
      ExpressionType nodeType = node.NodeType;
      if ((uint)(nodeType - 10) > 1u) {
        if (nodeType == ExpressionType.Not) {
          _isSearchCondition = true;
        } else {
          _isSearchCondition = false;
        }
      }
      Expression operand = Visit(node.Operand);
      _isSearchCondition = isSearchCondition;
      node = node.Update(operand);
      if (node.NodeType != ExpressionType.Convert && node.NodeType != ExpressionType.ConvertChecked) {
        return ApplyConversion(node);
      }
      return node;
    }

    protected override Expression VisitExtension(Expression node) {
      bool isSearchCondition = _isSearchCondition;
      _isSearchCondition = false;
      Expression expression = base.VisitExtension(node);
      _isSearchCondition = isSearchCondition;
      return ApplyConversion(expression);
    }

    protected override Expression VisitParameter(ParameterExpression node) {
      return ApplyConversion(node);
    }

    private Expression ApplyConversion(Expression expression) {
      if (!_isSearchCondition) {
        return ConvertToValue(expression);
      }
      return ConvertToSearchCondition(expression);
    }

     static bool IsSearchCondition(Expression expression) {
      expression = expression.RemoveConvert();
      if (!(expression is BinaryExpression) && expression.NodeType != ExpressionType.Not && expression.NodeType != ExpressionType.Extension) {
        return false;
      }
      if (!expression.IsComparisonOperation() && !expression.IsLogicalOperation() && !(expression is ExistsExpression) && !(expression is InExpression) && !(expression is IsNullExpression) && !(expression is LikeExpression)) {
        return expression is StringCompareExpression;
      }
      return true;
    }

     static Expression BuildCompareToExpression(Expression expression, bool compareTo) {
      BinaryExpression binaryExpression = Expression.Equal(expression, (Expression.Constant(compareTo, expression.Type)));
      if (!(expression.Type == typeof(bool))) {
        return Expression.Convert(binaryExpression, expression.Type);
      }
      return binaryExpression;
    }

     static Expression ConvertToSearchCondition(Expression expression) {
      if (!IsSearchCondition(expression)) {
        return BuildCompareToExpression(expression, true);
      }
      return expression;
    }

     static Expression ConvertToValue(Expression expression) {
      if (!IsSearchCondition(expression)) {
        return expression;
      }
      return Expression.Condition(expression, Expression.Constant(true, expression.Type), Expression.Constant(false, expression.Type));
    }
  }

}