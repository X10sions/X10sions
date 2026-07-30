using System.Globalization;

namespace System;
  public static class DecimalExtensions {

    public static decimal? DivideBy(this decimal value, decimal denominator) => denominator == 0 ? null : decimal.Divide(value, denominator);
    public static decimal? DivideBy(this decimal value, decimal? denominator) => !denominator.HasValue ? null : value.DivideBy(denominator.Value);
    public static decimal? DivideBy(this decimal? value, decimal denominator) => value?.DivideBy(denominator);
    public static decimal? DivideBy(this decimal? value, decimal? denominator) => value?.DivideBy(denominator);

    public static bool IsEven(this decimal value) => value % 2 == 0;
    public static bool IsOdd(this decimal value) => value % 2 != 0;

    public static string ToInvariant(this decimal value) => value.ToString(NumberFormatInfo.InvariantInfo);

    public static string SqlLiteral(this decimal value) => value.SqlLiteral(SqlDecimalOptions.Default);
    public static string SqlLiteral(this decimal? value) => value.SqlLiteral(SqlDecimalOptions.Default);
    public static string SqlLiteral(this decimal value, SqlDecimalOptions options) => options.LiteralPrefix + value + options.LiteralSuffix;
    public static string SqlLiteral(this decimal? value, SqlDecimalOptions options) => value.HasValue ? value.Value.SqlLiteral(options) : SqlOptions.SqlNullString;

    public static string ToStringInvariantCulture(this decimal value, string format) => value.ToString(format, CultureInfo.InvariantCulture);

  public static string ToString(this decimal d, string positiveFormat, string zeroFormat) => d.ToString(positiveFormat, positiveFormat, zeroFormat);

  public static string ToString(this decimal? d, string positiveFormat, string zeroFormat, string nullFormat) => d.ToString(positiveFormat, positiveFormat, zeroFormat, nullFormat);

  public static string ToString(this decimal d, string positiveFormat, string negativeFormat, string zeroFormat) => d switch {
    0 => zeroFormat,
    decimal dec => dec.ToString(d < 0 ? negativeFormat : positiveFormat)
  };

  public static string ToString(this decimal? d, string positiveFormat, string negativeFormat, string zeroFormat, string nullFormat) => d switch {
    null => nullFormat,
    decimal dec => dec.ToString(positiveFormat, negativeFormat, zeroFormat)
  };

}