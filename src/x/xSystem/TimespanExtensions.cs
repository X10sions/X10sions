namespace System {

  public static class TimeSpanExtensions {
    public static string FriendlyElapsedTimeString(int milliSeconds) {
      if (milliSeconds < 0)
        return string.Empty;
      if (milliSeconds < 60000)
        return "just now";
      if (milliSeconds < 3600000)
        return (milliSeconds / 60000).ToString() + "m ago";
      return (milliSeconds / 3600000).ToString() + "h ago";
    }

    public static string FriendlyElapsedTimeString(this TimeSpan elapsed) => FriendlyElapsedTimeString((int)elapsed.TotalMilliseconds);

    public static string ToSqlTime(this TimeSpan ts, int milliSecondsPrecision = 0) => ts.ToString(DateTimeConstants.SqlTimeFormat( milliSecondsPrecision));

    public static string SqlLiteralTime(this TimeSpan d) => d.SqlLiteralTime(new SqlTimeOptions());
    public static string SqlLiteralTime(this TimeSpan? d) => d.SqlLiteralTime(new SqlTimeOptions());
    public static string SqlLiteralTime(this TimeSpan d, SqlTimeOptions options) => options.LiteralPrefix + d.ToSqlTime(options.MilliSecondsPrecision) + options.LiteralSuffix;
    public static string SqlLiteralTime(this TimeSpan? d, SqlTimeOptions options) => d.HasValue ? d.Value.SqlLiteralTime(options) : SqlOptions.SqlNullString;

  }
}