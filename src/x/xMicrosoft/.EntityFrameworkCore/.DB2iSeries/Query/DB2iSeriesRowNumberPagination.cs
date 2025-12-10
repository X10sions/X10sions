using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Text;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query;

public static class DB2iSeriesRowNumberPagination {

  private static string BuildOrderBySql(IReadOnlyList<OrderingExpression> orderings) {
    var sb = new StringBuilder("ORDER BY ");
    for (int i = 0; i < orderings.Count; i++) {
      var ord = orderings[i];
      var colName = TryGetSimpleColumnName(ord.Expression) ?? throw new NotSupportedException("ROW_NUMBER rewriter only supports simple column orderings.");
      sb.Append(colName).Append(ord.IsAscending ? " ASC" : " DESC");
      if (i < orderings.Count - 1) sb.Append(", ");
    }
    return sb.ToString();
  }

  public static SelectExpression RewriteWithRowNumber(SelectExpression select, SqlExpression offset, SqlExpression? limit, ISqlExpressionFactory sql) {
    var orderings = select.Orderings.Count > 0 ? select.Orderings : new[] { new OrderingExpression(sql.Fragment("(SELECT 1)"), ascending: true) };
    var orderBySql = BuildOrderBySql(orderings);
    // ROW_NUMBER() OVER (ORDER BY …) as fragment
    var rowNumberFragment = sql.Fragment($"ROW_NUMBER() OVER ({orderBySql})");
    var rnAlias = "RN";
    var rnProjection = new ProjectionExpression(rowNumberFragment, rnAlias);
    var projections = select.Projection.ToList();
    projections.Add(rnProjection);
    // Reference RN column in predicate
    var rnColumn = new ColumnExpression(rnAlias, tableAlias: null, typeof(long), null, false);
    // RN >= offset+1
    var lowerBound = sql.GreaterThanOrEqual(rnColumn, sql.Add(offset, sql.Constant(1)));
    SqlExpression predicate = lowerBound;
    if (limit != null) {
      var upperBound = sql.LessThanOrEqual(rnColumn, sql.Add(offset, limit));
      predicate = sql.AndAlso(lowerBound, upperBound);
    }
    return select.Update(select.Tables,
      projections: projections,
      predicate: select.Predicate == null ? predicate : sql.AndAlso(select.Predicate, predicate),
      groupBy: select.GroupBy,
      having: select.Having,
      orderings: select.Orderings,
      limit: select.Limit,
      offset: select.Offset);
  }


  private static string? TryGetSimpleColumnName(SqlExpression expr) {
    if (expr is ColumnExpression col) {
      return !string.IsNullOrEmpty(col.TableAlias) ? $"\"{col.TableAlias}\".\"{col.Name}\"" : $"\"{col.Name}\"";
    }
    if (expr is SqlFragmentExpression frag) {
      return frag.Sql;
    }
    return null;
  }


}
