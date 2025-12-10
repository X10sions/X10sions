using Microsoft.EntityFrameworkCore.Query;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query;

public sealed class DB2iSeriesQueryTranslationPostprocessorFactory : IQueryTranslationPostprocessorFactory {
  private readonly QueryTranslationPostprocessorDependencies _deps;
  private readonly ISqlExpressionFactory _sql;

  public DB2iSeriesQueryTranslationPostprocessorFactory(QueryTranslationPostprocessorDependencies deps, ISqlExpressionFactory sql) {
    _deps = deps;
    _sql = sql;
  }

  public QueryTranslationPostprocessor Create(QueryCompilationContext ctx) => new DB2iSeriesQueryTranslationPostprocessor(_deps, _sql, ctx);
}