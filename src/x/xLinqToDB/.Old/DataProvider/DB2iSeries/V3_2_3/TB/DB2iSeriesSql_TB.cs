using LinqToDB;
using System;
using System.Globalization;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
  public class DB2iSeriesSql_TB {
    public enum _TrimType {
      Both,
      Leading,
      Trailing
    }

    [xConcatExpressionAttribute_TB] public static string Concat(params object[] args) => string.Concat(args);
    [xConcatExpressionAttribute_TB] public static string Concat(params string[] args) => string.Concat(args);

    //[xDatePartExpressionAttribute_TB("DB2.iSeries", "{{1}} + {0}", 60, true, new string[] {
    //  "{0} Year",
    //  "({0} * 3) Month",
    //  "{0} Month",
    //  "{0} Day",
    //  "{0} Day",
    //  "({0} * 7) Day",
    //  "{0} Day",
    //  "{0} Hour",
    //  "{0} Minute",
    //  "{0} Second",
    //  "({0} * 1000) Microsecond"
    //}, 0, new int[] {
    //  1,
    //  2
    //})]
    public static DateTime? DateAdd(Sql.DateParts part, double? number, DateTime? d) => part switch {
      Sql.DateParts.Year => d.Value.AddYears((int)Math.Round(number.Value)),
      Sql.DateParts.Quarter => d.Value.AddMonths((int)Math.Round(number.Value) * 3),
      Sql.DateParts.Month => d.Value.AddMonths((int)Math.Round(number.Value)),
      Sql.DateParts.DayOfYear => d.Value.AddDays(number.Value),
      Sql.DateParts.Day => d.Value.AddDays(number.Value),
      Sql.DateParts.Week => d.Value.AddDays(number.Value * 7.0),
      Sql.DateParts.WeekDay => d.Value.AddDays(number.Value),
      Sql.DateParts.Hour => d.Value.AddHours(number.Value),
      Sql.DateParts.Minute => d.Value.AddMinutes(number.Value),
      Sql.DateParts.Second => d.Value.AddSeconds(number.Value),
      Sql.DateParts.Millisecond => d.Value.AddMilliseconds(number.Value),
      _ => throw new InvalidOperationException(),
    };

    //[xDatePartExpressionAttribute_TB("DB2.iSeries", "{0}", false, new string[] {
    //  null,
    //  null,
    //  null,
    //  null,
    //  null,
    //  null,
    //  "DayOfWeek",
    //  null,
    //  null,
    //  null,
    //  null
    //}, 0, new int[] {
    //  1
    //})]
    public static int? DatePart(Sql.DateParts part, DateTime? d) => part switch {
      Sql.DateParts.Year => d.Value.Year,
      Sql.DateParts.Quarter => (int)Math.Round((d.Value.Month - 1) / 3.0 + 1.0),
      Sql.DateParts.Month => d.Value.Month,
      Sql.DateParts.DayOfYear => d.Value.DayOfYear,
      Sql.DateParts.Day => d.Value.Day,
      Sql.DateParts.Week => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d.Value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday),
      Sql.DateParts.WeekDay => unchecked((int)checked(d.Value.DayOfWeek + 1 + Sql.DateFirst + 6) % 7) + 1,
      Sql.DateParts.Hour => d.Value.Hour,
      Sql.DateParts.Minute => d.Value.Minute,
      Sql.DateParts.Second => d.Value.Second,
      Sql.DateParts.Millisecond => d.Value.Millisecond,
      _ => throw new InvalidOperationException(),
    };

    [Sql.Expression("DB2.iSeries", "zStrip({0}, T, {1})")] public static string TrimRight(string str, char? ch) => str != null && ch.HasValue ? str.TrimEnd(ch.Value) : null;
    [Sql.Expression("DB2.iSeries", "xStrip({0}, T, {1})")] public static string PadRight(string str, char? ch) => str != null && ch.HasValue ? str.TrimEnd(ch.Value) : null;

  }
}
