using System.Linq.Expressions;
using JetBrains.Annotations;
using xEFCore.xAS400.Query.Expressions.Internal;

namespace xEFCore.xAS400.Query.Sql.Internal {
  public interface IAS400ExpressionVisitor {
    Expression VisitDatePartExpression_DB2([NotNull] DatePartExpression_DB2 expression);
    Expression VisitDayOfYearExpression_DB2([NotNull] DayOfYearExpression_DB2 expression);
    Expression VisitRowNumber_DB2([NotNull] RowNumberExpression_DB2 expression);
    Expression VisitRowNumber_SqlServer([NotNull] RowNumberExpression_SqlServer expression);
    Expression VisitTrimFunction_DB2([NotNull] TrimFunctionExpression_DB2 expression);
  }
}