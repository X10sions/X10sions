using System.Globalization;

namespace System {
  public static class String_DateTimeExtensions {

    public static DateTime DateTime_Parse_yyMMdd(this string yyMMdd) => DateTime.ParseExact(yyMMdd, nameof(yyMMdd), CultureInfo.InvariantCulture);
    public static DateTime DateTime_Parse_HHmmss(this string HHmmss) => DateTime.ParseExact(HHmmss, nameof(HHmmss), CultureInfo.InvariantCulture);
    public static DateTime DateTime_Parse_yyMMddHHmmss(this string yyMMddHHmmss) => DateTime.ParseExact(yyMMddHHmmss, nameof(yyMMddHHmmss), CultureInfo.InvariantCulture);
    public static DateTime DateTime_Parse_yyMMddHHmmss(this string yyMMdd, string HHmmss) => (yyMMdd + HHmmss).DateTime_Parse_yyMMddHHmmss();
  }
}