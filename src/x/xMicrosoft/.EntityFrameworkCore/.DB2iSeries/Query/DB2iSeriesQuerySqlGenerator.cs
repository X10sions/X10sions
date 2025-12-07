using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query;

public class DB2iSeriesQuerySqlGenerator : QuerySqlGenerator {
  public DB2iSeriesQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies) : base(dependencies) { }

  protected override string GetOperator(SqlBinaryExpression binaryExpression) => base.GetOperator(binaryExpression);
}
