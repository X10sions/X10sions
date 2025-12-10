using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries;

public sealed class DB2iSeriesSqlExpressionFactory : SqlExpressionFactory {
  public DB2iSeriesSqlExpressionFactory(SqlExpressionFactoryDependencies deps) : base(deps) { }

  public override SqlExpression ApplyDefaultTypeMapping(SqlExpression expr) {
    var applied = base.ApplyDefaultTypeMapping(expr);

    // If expression is boolean in a projection, ensure SMALLINT conversion (0/1)
    if (applied.Type == typeof(bool)) {
      // CASE WHEN expr THEN 1 ELSE 0 END
      var one = Fragment("1");
      var zero = Fragment("0");
      return Case(new[] { new CaseWhenClause(applied, one) }, zero);
    }

    return applied;
  }
}