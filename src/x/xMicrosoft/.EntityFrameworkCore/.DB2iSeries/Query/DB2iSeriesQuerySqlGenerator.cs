using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Infrastructure;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query;

public class DB2iSeriesQuerySqlGenerator : QuerySqlGenerator {
  private readonly DB2iSeriesServerCapabilities _capabilities;

  public DB2iSeriesQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies, DB2iSeriesServerCapabilities capabilities) : base(dependencies) {
    _capabilities = capabilities;
  }

  protected override void GenerateLimitOffset(SelectExpression selectExpression) {
    // v5r4: support FETCH FIRST, avoid OFFSET
    if (selectExpression.Limit != null && selectExpression.Offset == null) {
      Sql.AppendLine().Append("FETCH FIRST ");
      Visit(selectExpression.Limit);
      Sql.Append(" ROWS ONLY");
      return;
    }

    if (selectExpression.Offset != null) {
      if (_capabilities.SupportsOffset) {
        Sql.AppendLine().Append("OFFSET ");
        Visit(selectExpression.Offset);
        Sql.Append(" ROWS");
        if (selectExpression.Limit != null) {
          Sql.Append(" FETCH NEXT ");
          Visit(selectExpression.Limit);
          Sql.Append(" ROWS ONLY");
        }
        return;
      }
      if (_capabilities.SupportsRowNumber) {
        throw new NotSupportedException("ROW_NUMBER emulation is not yet enabled. Enable keyset pagination or implement IQueryTranslationPostprocessor.");
      }
      throw new NotSupportedException("OFFSET pagination is not supported on this IBM i version.");
    }

    // If OFFSET requested, emulate via ROW_NUMBER() if available; otherwise, throw
    //if (selectExpression.Offset != null) {
    //  throw new NotSupportedException("OFFSET pagination is not supported on IBM i v5r4 via this provider.");
    //}
    base.GenerateLimitOffset(selectExpression);
  }


  protected override string GetOperator(SqlBinaryExpression binaryExpression) => base.GetOperator(binaryExpression);

  protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression) {
    // Map common functions to DB2 for i equivalents as needed
    // Example: LOWER/UPPER supported; DATE functions formatting may differ.

    //// LOWER(string)
    //var lower = _sql.Function("LCASE", new[] { instance }, typeof(string), nullable: true);


    //// SUBSTR(string, start, length)
    //var startPlusOne = _sql.Add(arguments[0], _sql.Constant(1));
    //var substr = _sql.Function("SUBSTR", new[] { instance, startPlusOne, arguments[1] }, typeof(string), nullable: true);

    //// ABS(number)
    //var abs = _sql.Function("ABS", new[] { arguments[0] }, typeof(double), nullable: false);


    //// YEAR(date)
    //var year = _sql.Function("YEAR", new[] { instance }, typeof(int), nullable: false);

  //  var rowNumber = new WindowFunctionExpression(
  //functionName: "ROW_NUMBER",
  //arguments: Array.Empty<SqlExpression>(),
  //type: typeof(long),
  //typeMapping: null,
  //partitions: null,
  //orderings: select.Orderings
  //);


    return base.VisitSqlFunction(sqlFunctionExpression);
  }

}
