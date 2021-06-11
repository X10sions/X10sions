using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using System.Linq;
using System.Linq.Expressions;
using xEFCore.xAS400.Query.Expressions.Internal;

namespace xEFCore.xAS400.Query.Sql.Internal.Visitors {
  class RowNumberPagingExpressionVisitor_DB2 : ExpressionVisitorBase {

    const string RowNumberColumnName = "row_number";
    int _counter;

    public override Expression Visit(Expression node) {
      ExistsExpression existsExpression;
      if ((existsExpression = (node as ExistsExpression)) != null) {
        return VisitExistExpression(existsExpression);
      }
      SelectExpression selectExpression;
      if ((selectExpression = (node as SelectExpression)) == null) {
        return base.Visit(node);
      }
      return VisitSelectExpression(selectExpression);
    }

    private static bool RequiresRowNumberPaging(SelectExpression selectExpression) {
      if (selectExpression.Offset != null) {
        return !selectExpression.Projection.Any((Expression p) => p is RowNumberExpression_DB2);
      }
      return false;
    }

    Expression VisitSelectExpression(SelectExpression selectExpression) {
      base.Visit(selectExpression);
      if (!RequiresRowNumberPaging(selectExpression)) {
        return selectExpression;
      }
      SelectExpression selectExpression2 = selectExpression.PushDownSubquery();
      foreach (Expression item in selectExpression2.Projection) {
        selectExpression.AddToProjection(item.LiftExpressionFromSubquery(selectExpression2), true);
      }
      AliasExpression aliasExpression = new AliasExpression("row_number" + ((_counter != 0) ? $"{_counter}" : ""), new RowNumberExpression_DB2(selectExpression2.OrderBy));
      _counter++;
      selectExpression2.ClearOrderBy();
      selectExpression2.AddToProjection(aliasExpression, false);
      ColumnReferenceExpression left = new ColumnReferenceExpression(aliasExpression, selectExpression2);
      Expression expression = selectExpression2.Offset ?? Expression.Constant(0);
      if (selectExpression2.Offset != null) {
        selectExpression.AddToPredicate(Expression.GreaterThan(left, expression));
        selectExpression2.Offset = null;
      }
      if (selectExpression2.Limit != null) {
        object obj = (selectExpression2.Limit as ConstantExpression)?.Value;
        object obj2 = (expression as ConstantExpression)?.Value;
        Expression right = (obj != null && obj2 != null) ? ((Expression)Expression.Constant((int)obj2 + (int)obj)) : ((Expression)Expression.Add(expression, selectExpression2.Limit));
        selectExpression.AddToPredicate(Expression.LessThanOrEqual(left, right));
        selectExpression2.Limit = null;
      }
      return selectExpression;
    }

     Expression VisitExistExpression(ExistsExpression existsExpression) {
      SelectExpression selectExpression = (SelectExpression)Visit(existsExpression.Subquery);
      if (selectExpression.Limit == null && selectExpression.Offset == null) {
        selectExpression.ClearOrderBy();
      }
      return new ExistsExpression(selectExpression);
    }
  }

}