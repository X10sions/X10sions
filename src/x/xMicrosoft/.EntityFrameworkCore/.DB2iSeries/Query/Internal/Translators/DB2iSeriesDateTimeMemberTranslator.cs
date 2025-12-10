using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query.Internal;

class DB2iSeriesDateTimeMemberTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMemberTranslator {

  public SqlExpression? Translate(SqlExpression instance, MemberInfo member, Type returnType, IDiagnosticsLogger<DbLoggerCategory.Query> logger) {
    if (member.DeclaringType == typeof(DateTime)) {
      if (member.Name == nameof(DateTime.Now)) return sqlExpressionFactory.CurrentTimestampSqlExpression();
      if (member.Name == nameof(DateTime.Year)) return sqlExpressionFactory.Function("YEAR", [instance], true, [true], typeof(int));
      if (member.Name == nameof(DateTime.Month)) return sqlExpressionFactory.Function("MONTH", new[] { instance }, true, [true], typeof(int));
      if (member.Name == nameof(DateTime.Day)) return sqlExpressionFactory.Function("DAY", new[] { instance }, true, [true], typeof(int));
    }
    return null;
  }

}

public static class SqlExpressionFactoryExtensions {
  public static SqlExpression? CurrentTimestampSqlExpression(this ISqlExpressionFactory sql) => sql.Function("CURRENT_TIMESTAMP", Array.Empty<SqlExpression>(), true, [true], typeof(DateTime));

}