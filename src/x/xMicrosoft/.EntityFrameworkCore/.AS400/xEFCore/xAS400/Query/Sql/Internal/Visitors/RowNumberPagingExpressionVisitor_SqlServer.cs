using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using Remotion.Linq.Clauses;
using xEFCore.xAS400.Query.Expressions.Internal;

namespace xEFCore.xAS400.Query.Sql.Internal.Visitors {
  class RowNumberPagingExpressionVisitor_SqlServer : ExpressionVisitorBase {

    const string RowNumberColumnName = "__RowNumber__";
    int _counter;

    public override Expression Visit(Expression expression) {
      if (expression is ExistsExpression existsExpression) {
        return VisitExistExpression(existsExpression);
      }
      return expression is SelectExpression selectExpression
          ? VisitSelectExpression(selectExpression)
          : base.Visit(expression);
    }

    static bool RequiresRowNumberPaging(SelectExpression selectExpression)
       => selectExpression.Offset != null
          && !selectExpression.Projection.Any(p => p is RowNumberPagingExpressionVisitor_SqlServer);

    Expression VisitSelectExpression(SelectExpression selectExpression) {
      base.Visit(selectExpression);
      if (!RequiresRowNumberPaging(selectExpression)) {
        return selectExpression;
      }
      var subQuery = selectExpression.PushDownSubquery();
      foreach (var projection in subQuery.Projection) {
        selectExpression.AddToProjection(projection.LiftExpressionFromSubquery(subQuery));
      }
      if (subQuery.OrderBy.Count == 0) {
        subQuery.AddToOrderBy(
            new Ordering(new SqlFunctionExpression("@@RowCount", typeof(int)), OrderingDirection.Asc));
      }
      var innerRowNumberExpression = new AliasExpression(
          RowNumberColumnName + (_counter != 0 ? $"{_counter}" : ""),
          new RowNumberExpression_SqlServer(subQuery.OrderBy));
      _counter++;
      subQuery.ClearOrderBy();
      subQuery.AddToProjection(innerRowNumberExpression,  false);
      var rowNumberReferenceExpression = new ColumnReferenceExpression(innerRowNumberExpression, subQuery);
      var offset = subQuery.Offset ?? Expression.Constant(0);
      if (subQuery.Offset != null) {
        selectExpression.AddToPredicate
            (Expression.GreaterThan(rowNumberReferenceExpression, offset));
        subQuery.Offset = null;
      }
      if (subQuery.Limit != null) {
        var constantValue = (subQuery.Limit as ConstantExpression)?.Value;
        var offsetValue = (offset as ConstantExpression)?.Value;
        var limitExpression
            = constantValue != null
              && offsetValue != null
                ? (Expression)Expression.Constant((int)offsetValue + (int)constantValue)
                : Expression.Add(offset, subQuery.Limit);
        selectExpression.AddToPredicate(
            Expression.LessThanOrEqual(rowNumberReferenceExpression, limitExpression));
        subQuery.Limit = null;
      }
      return selectExpression;
    }

    Expression VisitExistExpression(ExistsExpression existsExpression) {
      var newSubquery = (SelectExpression)Visit(existsExpression.Subquery);
      if (newSubquery.Limit == null && newSubquery.Offset == null) {
        newSubquery.ClearOrderBy();
      }
      return new ExistsExpression(newSubquery);
    }

  }
}