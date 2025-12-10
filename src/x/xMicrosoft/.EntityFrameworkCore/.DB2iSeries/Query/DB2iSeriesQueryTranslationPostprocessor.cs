using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query;

public sealed class DB2iSeriesQueryTranslationPostprocessor : QueryTranslationPostprocessor {
  private readonly ISqlExpressionFactory _sql;

  public DB2iSeriesQueryTranslationPostprocessor(
      QueryTranslationPostprocessorDependencies deps,
      ISqlExpressionFactory sql,
      QueryCompilationContext queryCompilationContext) : base(deps, queryCompilationContext) {
    _sql = sql;
  }

  public override Expression Process(Expression query) {
    var processed = base.Process(query);
    if (processed is SelectExpression select && select.Offset != null) {
      return DB2iSeriesRowNumberPagination.RewriteWithRowNumber(select, select.Offset!, select.Limit, _sql);
    }
    return processed;
  }

}
