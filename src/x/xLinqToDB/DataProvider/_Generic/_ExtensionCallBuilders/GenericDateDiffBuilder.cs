using LinqToDB.SqlQuery;
using System;

namespace LinqToDB.DataProvider.DB2iSeries {
  public class DB2iSeriesDateDiffBuilder : Sql.IExtensionCallBuilder {
    public void Build(Sql.ISqExtensionBuilder builder) {
      var startDate = builder.GetExpression(1);
      var endDate = builder.GetExpression(2);
      var secondsExpr = builder.Mul<int>(builder.Sub<int>(new SqlFunction(typeof(int), "Days", endDate), new SqlFunction(typeof(int), "Days", startDate)), new SqlValue(86400));
      var midnight = builder.Sub<int>(new SqlFunction(typeof(int), "MIDNIGHT_SECONDS", endDate), new SqlFunction(typeof(int), "MIDNIGHT_SECONDS", startDate));
      var resultExpr = builder.Add<int>(secondsExpr, midnight);
      resultExpr = builder.GetValue<Sql.DateParts>(0) switch {
        Sql.DateParts.Day => builder.Div(resultExpr, 86400),
        Sql.DateParts.Hour => builder.Div(resultExpr, 3600),
        Sql.DateParts.Minute => builder.Div(resultExpr, 60),
        Sql.DateParts.Millisecond => builder.Add<int>(builder.Mul(resultExpr, 1000), builder.Div(builder.Sub<int>(new SqlFunction(typeof(int), "MICROSECOND", endDate), new SqlFunction(typeof(int), "MICROSECOND", startDate)), 1000)),
        _ => throw new ArgumentOutOfRangeException()
      };
      builder.ResultExpression = resultExpr;
    }
  }

}