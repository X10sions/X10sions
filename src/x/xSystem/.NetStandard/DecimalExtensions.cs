using System.Globalization;

namespace System {
  public static class DecimalExtensions {

    public static decimal? DivideBy(this decimal value, decimal denominator) => denominator == 0 ? null : decimal.Divide(value, denominator);
    public static decimal? DivideBy(this decimal value, decimal? denominator) => !denominator.HasValue ? null : value.DivideBy(denominator.Value);
    public static decimal? DivideBy(this decimal? value, decimal denominator) => value?.DivideBy(denominator);
    public static decimal? DivideBy(this decimal? value, decimal? denominator) => value?.DivideBy(denominator);

    public static bool IsEven(this decimal value) => value % 2 == 0;
    public static bool IsOdd(this decimal value) => value % 2 != 0;

    public static string ToInvariant(this decimal value) => value.ToString(NumberFormatInfo.InvariantInfo);

    public static string SqlLiteral(this decimal value) => value.SqlLiteral(new SqlDecimalOptions());
    public static string SqlLiteral(this decimal? value) => value.SqlLiteral(new SqlDecimalOptions());
    public static string SqlLiteral(this decimal value, SqlDecimalOptions options) => options.LiteralPrefix + value + options.LiteralSuffix;
    public static string SqlLiteral(this decimal? value, SqlDecimalOptions options) => value.HasValue ? value.Value.SqlLiteral(options) : SqlOptions.SqlNullString;


  }
}