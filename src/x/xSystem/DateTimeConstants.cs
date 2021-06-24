namespace System {
  public static class DateTimeConstants {
    public const string IsoDateFormat = "yyyy-MM-dd";
    public const string IsoTimeFormat = "HH:mm:ss";
    public const string DateFormat_YYYYMMDD = "yyyyMMdd";
    public const string FileDateFormat = DateFormat_YYYYMMDD;
    public const string FileTimestampFormat = "yyyyMMdd-HHmmss";
    public const string JavascriptTimestampFormat = "yyyy-MM-dd'T'HH:mm:ss";
    public const string DefaultDateFormat = "dd-MMM-yyyy";
    public const string DefaultTimeFormat = IsoTimeFormat;
    public static string IsoTimestampFormat(int milliSecondsPrecision = 0) => $"{IsoDateFormat}T{IsoTimeFormat}{MilliSecondsFormat(milliSecondsPrecision)}";
    public const string IsoTimestampFormatUTC = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";
    public const string IsoTimestampFormatLocal = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff";
    public static string DefaultTimestampFormat(int milliSecondsPrecision = 0) => $"{DefaultDateFormat} {DefaultTimeFormat}{MilliSecondsFormat(milliSecondsPrecision)}";
    public static string MilliSecondsFormat(int milliSecondsPrecision, string prefix = SqlMilliSecondsPrefixDefault) => milliSecondsPrecision > 0 ? prefix + new string('f', milliSecondsPrecision) : string.Empty;
    public const string TimeFormat_HHMMSS = "HHmmss";
    public static string TimeSpanFormat(int milliSecondsPrecision = 0) => $"d.hh.mm.ss{MilliSecondsFormat(milliSecondsPrecision)}";
    public const string SqlDateFormat = IsoDateFormat;
    public const string SqlDateTimeSeparator = " ";
    public const string SqlMilliSecondsPrefixDefault = ".";
    public static string SqlTimeFormat(int milliSecondsPrecision = 0) => IsoTimeFormat + MilliSecondsFormat(milliSecondsPrecision);
    public static string SqlTimestampFormat(int milliSecondsPrecision = 0, string dateTimeSeparator = SqlDateTimeSeparator) => SqlDateFormat + dateTimeSeparator + SqlTimeFormat(milliSecondsPrecision);
  }
}