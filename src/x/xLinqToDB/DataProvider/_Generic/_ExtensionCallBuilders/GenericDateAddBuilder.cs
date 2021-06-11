using LinqToDB.SqlQuery;
using System;

namespace LinqToDB.DataProvider.DB2iSeries {
  public class DB2iSeriesDateAddBuilder : Sql.IExtensionCallBuilder {

    public void Build(Sql.ISqExtensionBuilder builder) {
      var expStr = builder.GetValue<Sql.DateParts>("part") switch {
        Sql.DateParts.Year => "{0} + {1} Year",
        Sql.DateParts.Quarter => "{0} + ({1} * 3) Month",
        Sql.DateParts.Month => "{0} + {1} Month",
        Sql.DateParts.DayOfYear => "{0} + {1} Day",
        Sql.DateParts.Day => "{0} + {1} Day",
        Sql.DateParts.WeekDay => "{0} + {1} Day",
        Sql.DateParts.Week => "{0} + ({1} * 7) Day",
        Sql.DateParts.Hour => "{0} + {1} Hour",
        Sql.DateParts.Minute => "{0} + {1} Minute",
        Sql.DateParts.Second => "{0} + {1} Second",
        Sql.DateParts.Millisecond => "{0} + ({1} * 1000) Microsecond",
        _ => throw new ArgumentOutOfRangeException()
      };
      var date = builder.GetExpression("date");
      var number = builder.GetExpression("number");
      builder.ResultExpression = new SqlExpression(typeof(DateTime?), expStr, 60, date, number);
    }
  }

}