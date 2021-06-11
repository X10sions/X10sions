using System;

namespace LinqToDB.DataProvider.DB2iSeries{
  public class DB2iSeriesDatePartBuilder: Sql.IExtensionCallBuilder {
    public void Build(Sql.ISqExtensionBuilder builder) {
      var partStr = (builder.GetValue<Sql.DateParts>("part")) switch {
        Sql.DateParts.Year => "YEAR({date})",
        Sql.DateParts.Quarter => "QUARTER({date})",
        Sql.DateParts.Month => "MONTH({date})",
        Sql.DateParts.DayOfYear => "DAYOFYEAR({date})",
        Sql.DateParts.Day => "DAY({date})",
        Sql.DateParts.Week => "DAYOFYEAR({date}) / 7",
        Sql.DateParts.WeekDay => "DAYOFWEEK({date})",
        Sql.DateParts.Hour => "HOUR({date})",
        Sql.DateParts.Minute => "MINUTE({date})",
        Sql.DateParts.Second => "SECOND({date})",
        Sql.DateParts.Millisecond => "MICROSECOND({date}) / 1000",
        _ => throw new ArgumentOutOfRangeException(),
      };
      builder.Expression = partStr;
    }
  }

}