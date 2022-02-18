using System.Globalization;

namespace System {
  public static class DateTimeExtensions {

    public static string InvariantSortableDateTimePattern = DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern;
    public static string CurrentSortableDateTimePattern = DateTimeFormatInfo.CurrentInfo.SortableDateTimePattern;

    public static DateTime AddWeekdays(this DateTime d, int days) {
      var sign = days < 0 ? -1 : 1;
      var weekdaysAdded = 0;
      while (weekdaysAdded < Math.Abs(days)) {
        d = d.AddDays(sign);
        if (d.DayOfWeek.IsWeekDay()) {
          weekdaysAdded++;
        }
      }
      return d;
    }

    public static DateTime EndOfDay(this DateTime date) => date.Date.AddDays(1).AddMilliseconds(-1);
    public static DateTime EndOfMonth(this DateTime d) => new DateTime(d.Year, d.Month, DateTime.DaysInMonth(d.Year, d.Month), 23, 59, 59, 999);
    public static DateTime FirstDayOfMonth(this DateTime d) => new DateTime(d.Year, d.Month, 1);
    public static DateTime FirstDayOfWeek(this DateTime d, DayOfWeek? dayOfWeek = null) {
      dayOfWeek = dayOfWeek ?? DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;
      while (d.DayOfWeek != dayOfWeek.Value) {
        d = d.AddDays(-1);
      }
      return d;
    }
    public static DateTime FirstDayOfYear(this DateTime d) => new DateTime(d.Year, 1, 1);

    public static string FriendlyDateString(this DateTime date, bool showTime) {
      if (date < DateTime.MinValue)
        return string.Empty;
      var FormattedDate = string.Empty;
      if (date.Date.Equals(DateTime.Today))
        FormattedDate = "Today";
      else if (date.Date == DateTime.Today.AddDays(-1))
        FormattedDate = "Yesterday";
      else if (date.Date > DateTime.Today.AddDays(-6))
        // Show the Day of the week
        FormattedDate = date.ToString("dddd");
      else
        FormattedDate = date.ToString("MMMM dd, yyyy");
      if (showTime)
        FormattedDate += " @  " + date.ToString("t").ToLower().Replace(" ", "");
      return FormattedDate;
    }

    public static bool IsBetween(this DateTime date, DateTime startDate, DateTime endDate) {
      var ticks = date.Ticks;
      return ticks >= startDate.Ticks && ticks <= endDate.Ticks;
    }

    public static bool IsWeekend(this DateTime dt) => dt.DayOfWeek.IsWeekend();
    public static bool IsWeekDay(this DateTime dt) => !dt.IsWeekend();

    public static long MonthInteger(this DateTime d) => (d.Year * 12) + d.Month;

    public static string ToSqlDate(this DateTime d) => d.ToString(DateTimeConstants.SqlDateFormat);
    public static string ToSqlTime(this DateTime d, int milliSecondsPrecision = 0) => d.ToString(DateTimeConstants.SqlTimeFormat(milliSecondsPrecision));
    public static string ToSqlTimestamp(this DateTime d, int milliSecondsPrecision = 0) => d.ToString(DateTimeConstants.SqlTimestampFormat(milliSecondsPrecision));

    public static string SqlLiteralDate(this DateTime d, string prefix = SqlDateOptions.DefaultLiteralPrefix, string suffix =SqlDateOptions.DefaultLiteralSuffix) => prefix + d.ToSqlDate() + suffix;
    public static string SqlLiteralDate(this DateTime d) => d.SqlLiteralDate(new SqlDateOptions());
    public static string SqlLiteralDate(this DateTime? d) => d.SqlLiteralDate(new SqlDateOptions());
    public static string SqlLiteralDate(this DateTime d, SqlDateOptions options) =>  d.SqlLiteralDate(options.LiteralPrefix, options.LiteralSuffix  ) ;
    public static string SqlLiteralDate(this DateTime? d, SqlDateOptions options) => d.HasValue ? d.Value.SqlLiteralDate(options) : SqlOptions.SqlNullString;

    public static string SqlLiteralTime(this DateTime d, int milliSecondsPrecision = 0, string prefix = SqlTimeOptions.DefaultLiteralPrefix, string suffix = SqlTimeOptions.DefaultLiteralSuffix) => prefix + d.ToSqlTime(milliSecondsPrecision) + suffix;
    public static string SqlLiteralTime(this DateTime d) => d.SqlLiteralTime(new SqlTimeOptions());
    public static string SqlLiteralTime(this DateTime? d) => d.SqlLiteralTime(new SqlTimeOptions());
    public static string SqlLiteralTime(this DateTime d, SqlTimeOptions options) => d.SqlLiteralTime (options.MilliSecondsPrecision, options.LiteralPrefix , options.LiteralSuffix);
    public static string SqlLiteralTime(this DateTime? d, SqlTimeOptions options) => d.HasValue ? d.Value.SqlLiteralTime(options) : SqlOptions.SqlNullString;

    public static string SqlLiteralTimestamp(this DateTime d, int milliSecondsPrecision = 0, string prefix = SqlTimestampOptions.DefaultLiteralPrefix, string suffix = SqlTimestampOptions.DefaultLiteralSuffix) => prefix + d.ToSqlTimestamp(milliSecondsPrecision) + suffix;
    public static string SqlLiteralTimestamp(this DateTime d) => d.SqlLiteralTimestamp(new SqlTimestampOptions());
    public static string SqlLiteralTimestamp(this DateTime? d) => d.SqlLiteralTimestamp(new SqlTimestampOptions());
    public static string SqlLiteralTimestamp(this DateTime d, SqlTimestampOptions options) => d.SqlLiteralTimestamp(options.MilliSecondsPrecision, options.LiteralPrefix , options.LiteralSuffix);
    public static string SqlLiteralTimestamp(this DateTime? d, SqlTimestampOptions options) => d.HasValue ? d.Value.SqlLiteralTimestamp(options) : SqlOptions.SqlNullString;

    public static DateTime LastDayOfMonth(this DateTime d) => new DateTime(d.Year, d.Month, 1).EndOfMonth();
    public static DateTime LastDayOfYear(this DateTime d) => new DateTime(d.Year, 12, 1).EndOfMonth();

    //    public static DateTime RoundDown(this DateTime d, int minutes) => new DateTime(d.Year, d.Month, d.Day, d.Hour, (d.Minute / minutes) * minutes, 0);
    public static DateTime SetTime(this DateTime d, int hour = 0, int minute = 0, int second = 0, int millisecond = 0) => new DateTime(d.Year, d.Month, d.Day, hour, minute, second, millisecond);
    public static DateTime StartOfDay(this DateTime date) => date.Date;
    public static DateTime StartOfMonth(this DateTime d) => new DateTime(d.Year, d.Month, 1, 0, 0, 0, 0);

    public static string ToDateAndTime(this DateTime? d, string dateFormat = DateTimeConstants.DefaultDateFormat, string timeFormat = DateTimeConstants.DefaultTimeFormat) => d.HasValue ? d.ToDateOnly(dateFormat) + " " + d.ToTimeOnly(timeFormat) : string.Empty;
    public static string ToDateOnly(this DateTime? d, string format = DateTimeConstants.DefaultDateFormat) => d.HasValue ? d.Value.ToString(format) : string.Empty;
    public static string ToDDMMMYYYY(this DateTime d) => d.ToString("dd-MMM-yyyy");
    public static string ToFileDate(this DateTime d) => d.ToString(DateTimeConstants.FileDateFormat);
    public static string ToFileTimestamp(this DateTime d) => d.ToString(DateTimeConstants.FileTimestampFormat);
    public static int? ToHHMMSS(this DateTime? d) => d.HasValue ?  (d.Value.Hour * 10000 + d.Value.Minute * 100 + d.Value.Second) : null;
    public static string ToJavascriptTimestamp(this DateTime d) => d.ToString(DateTimeConstants.JavascriptTimestampFormat);
    public static string ToTimeOnly(this DateTime? d, string format = DateTimeConstants.DefaultTimeFormat) => d.HasValue ? d.Value.ToString(format) : string.Empty;
    public static int ToYYYYMMDD(this DateTime d) => d.Year * 1000 + d.Month * 100 + d.Day;
    public static string ToYYYYMMDD_HHMMSS(this DateTime d, string separator = "-") => d.ToString(string.Concat(DateTimeConstants.DateFormat_YYYYMMDD, separator, DateTimeConstants.TimeFormat_HHMMSS));
    public static string ToYYYYMMDD_HHMMSS_FFFFFF(this DateTime d, string separator = "-", int milliSecondsPrecision = 6) => d.ToYYYYMMDD_HHMMSS(separator) + d.ToString(DateTimeConstants.MilliSecondsFormat(milliSecondsPrecision, separator));

    public static int WholeMonthsBetween(this DateTime d, DateTime maxDate) => maxDate.MonthInteger() - d.MonthInteger() - d.Day > maxDate.Day ? 1 : 0;
    public static int WholeYearsBetween(this DateTime d, DateTime maxDate) => d.WholeMonthsBetween(maxDate) / 12;

    #region "iSeries/iDb2Date Dates"

    public static int ToIntC(this DateTime d) => d.ToIntCYY() / 100;
    public static int ToIntCYY(this DateTime d) => d.Year - 1900;    
    public static int ToIntCYYMM(this DateTime d) => d.ToIntCYY() * 100 + d.Month;    
    public static int ToIntCYYMMDD(this DateTime d, int? day = null) => d.ToIntCYYMM() * 100 + (day ?? d.Day);
    public static int? ToIntCYYMMDD(this DateTime? d, int? day = null) => d.HasValue ?  d.Value.ToIntCYYMMDD(day) : null;
    public static int ToIntCYYMM00(this DateTime d) => d.ToIntCYYMMDD(0);
    public static int ToIntCYYMM01(this DateTime d) => d.ToIntCYYMMDD(1);
    public static int ToIntCYYMM99(this DateTime d) => d.ToIntCYYMMDD(99);
    public static int ToIntDDMMYY(this DateTime d) => d.Day * 10000 + d.Month * 100 + (d.Year % 100);
    public static decimal ToDecimalCYYMMDD_HHMMSS(this DateTime d, int? day, int? second = null) => d.ToIntCYYMMDD(day) + (d.ToIntHHMMSS(second) / 1000000);
    public static int ToIntHHMM(this DateTime d) => d.Hour * 100 + d.Minute;
    public static int ToIntHHMMSS(this DateTime d, int? second = null) => d.ToIntHHMM() * 100 + (second ?? d.Second);

    public static long ToLongYYYYMMDDHHMMSS(this DateTime d) => (d.ToYYYYMMDD() * 1000000) + d.ToIntHHMMSS();

    #endregion "iSeries"

    #region "Fiscal / Financial Year"

    public static DateTime FiscalEndDate(this DateTime d, int fiscalStartMonth = 7) => d.FiscalStartDate(fiscalStartMonth).AddYears(1).AddDays(-1);
    public static int FiscalEndYear(this DateTime d, int fiscalStartMonth = 7) => d.Year + (fiscalStartMonth > d.Month || fiscalStartMonth == 1 ? 0 : 1);
    public static int FiscalPeriod(this DateTime d, int fiscalStartMonth = 7) => d.Month - fiscalStartMonth + 1 + ((fiscalStartMonth > d.Month ? 1 : 0) * 12);
    public static DateTime FiscalStartDate(this DateTime d, int fiscalStartMonth = 7) => new DateTime(d.FiscalStartYear(fiscalStartMonth), fiscalStartMonth, 1);
    public static int FiscalStartYear(this DateTime d, int fiscalStartMonth = 7) => d.Year - (fiscalStartMonth > d.Month ? 1 : 0);
    public static int FiscalToCYY(this DateTime d, int fiscalStartMonth = 7) => d.FiscalEndYear(fiscalStartMonth) - 1900;
    public static int FiscalToCYYPP(this DateTime d, int fiscalStartMonth = 7) => (d.FiscalToCYY(fiscalStartMonth) * 100) + FiscalPeriod(d, fiscalStartMonth);

    #endregion "Fiscal / Financial Year"

    #region "ISO"

    public static int ToIsoTimestampLocal(this DateTime d) => Convert.ToInt32(d.ToString(DateTimeConstants.IsoTimestampFormatLocal));
    public static int ToIsoTimestampUTC(this DateTime d) => Convert.ToInt32(d.ToString(DateTimeConstants.IsoTimestampFormatUTC));

    #endregion "ISO"

    #region timezone

    //private static string timeZoneId = ConfigurationManager.AppSettings("TimeZoneId") ?? "W. Europe Standard Time";
    //public static DateTime ToLocalTime(this DateTime d) => TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(d, DateTimeKind.Utc), TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
    //public static DateTime ToUtcTime(this DateTime d) => TimeZoneInfo.ConvertTimeToUtc(d, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));

    #endregion timezone

    #region mtg.NetFramework.UDrive
    //private static string timeZoneId = ConfigurationManager.AppSettings["TimeZoneId"] ?? "W. Europe Standard Time";
    //public static bool IsWeekEnd(this DateTime d) => d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday;
    //public static DateTime RoundDown(this DateTime d, int minutes) => new DateTime(d.Year, d.Month, d.Day, d.Hour, unchecked(d.Minute / minutes) * minutes, 0);
    //public static int ToCYY(this DateTime d) => IntCYYMMDD.ToCYY(d);
    //public static int ToCYYMM(this DateTime d) => IntCYYMMDD.ToCYYMM(d);
    //public static int? ToCYYMM(this DateTime? d, DateTime? defaultIfNull = null) => (int?)(d.HasValue ? (ValueType)d.Value.ToCYYMM() : defaultIfNull);
    //public static int ToCYYMM00(this DateTime d) => IntCYYMMDD.ToCYYMM00(d);
    //public static int ToCYYMM01(this DateTime d) => IntCYYMMDD.ToCYYMM01(d);
    //public static int ToCYYMM99(this DateTime d) => IntCYYMMDD.ToCYYMM99(d);
    //public static int ToCYYMMDD(this DateTime d) => IntCYYMMDD.ToCYYMMDD(d);
    //public static int? ToCYYMMDD(this DateTime? d) => d.HasValue ? ToCYYMMDD(d.Value) : 0;
    //public static Expression<Func<DateTime, decimal>> ToCYYMMDD_Expression() => (DateTime d) => IntCYYMMDD.ToCYYMMDD(d);
    //public static decimal ToCYYMMDD_HHMMSS(this DateTime d) => IntCYYMMDD_HHMMSS.ToCYYMMDD_HHMMSS(d);
    //public static decimal? ToCYYMMDD_HHMMSS(this DateTime? d) => (decimal?)(d.HasValue ? (ValueType)d.Value.ToCYYMMDD_HHMMSS() : 0.0);
    //public static int ToHHMMSS(this DateTime d) => IntHHMMSS.ToHHMMSS(d);
    //public static string ToJavascript(this DateTime d) => d.ToString("yyyy-MM-dd'T'HH:mm:ss");
    //public static string ToJsonIso(this DateTime d) => JsonConvert.SerializeObject(d);
    //public static string ToJsonJavascript(this DateTime d) => JsonConvert.SerializeObject(d, new JavaScriptDateTimeConverter());
    //public static DateTime ToLocalTime(this DateTime d) {
    //  var destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    //  return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(d, DateTimeKind.Utc), destinationTimeZone);
    //}
    //public static string ToSqlDateExpression(this DateTime d) => $"'{ToSqlDate(d)}'";
    //public static string ToSqlDateExpression(this DateTime? d) => d.HasValue ? ToSqlDateExpression(d.Value) : "Null";
    //public static string ToSqlTime(this DateTime d) => d.ToString("hh:mm:ss");
    //public static string ToSqlTimeExpression(this DateTime d) => $"'{ToSqlTime(d)}'";
    //public static string ToSqlTimeExpression(this DateTime? d) => d.HasValue ? ToSqlTimeExpression(d.Value) : "Null";
    //public static string ToSqlTimestamp(this DateTime d) => ToSqlTimestamp(d, 0);
    //public static string ToSqlTimestampExpression(this DateTime d, int milliSecondsPrecision) => $"'{ToSqlTimestamp(d, milliSecondsPrecision)}'";
    //public static string ToSqlTimestampExpression(this DateTime? d, int milliSecondsPrecision) => d.HasValue ? ToSqlTimestampExpression(d.Value, milliSecondsPrecision) : "Null";
    //public static DateTime ToUtcTime(this DateTime d) {
    //  var sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    //  return TimeZoneInfo.ConvertTimeToUtc(d, sourceTimeZone);
    //}
    //public static decimal ToYYYYMMDD_HHMMSS_Decimal(this DateTime d) => Convert.ToDecimal(ToYYYYMMDD_HHMMSS(d, "."));
    //public static long ToYYYYMMDDHHMMSS(this DateTime d) => Convert.ToInt64(ToYYYYMMDD_HHMMSS(d, ""));
    #endregion

  }
}