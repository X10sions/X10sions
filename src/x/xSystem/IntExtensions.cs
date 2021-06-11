using System.Globalization;

namespace System {
  public static class IntExtensions {

    public static int GetArraySizeRequiredToEncode(this int count) {
      var numWholeOrPartialInputBlocks = checked(count + 2) / 3;
      return checked(numWholeOrPartialInputBlocks * 4);
    }

    public static bool IsEven(this int value) => value % 2 == 0;
    public static bool IsOdd(this int value) => value % 2 != 0;

    public static string Ordinal(this int num) {
      switch (num % 100) {
        case -11:
        case 11:
        case -12:
        case 12:
        case -13:
        case 13:
          return num + "th";
      }
      switch (num % 10) {
        case 1: return num + "st";
        case 2: return num + "nd";
        case 3: return num + "rd";
        default: return num + "th";
      }
    }

    public static string ToInvariant(this int value) => value.ToString(NumberFormatInfo.InvariantInfo);

    public static string SqlLiteral(this int value) => value.ToString();
    public static string SqlLiteral(this int? value) => value.HasValue ? value.Value.SqlLiteral() : SqlOptions.SqlNullString;

    //[Obsolete("Use SqLiteral()")] public static string ToSqlExpression(this int? @this) => @this.HasValue ? @this.Value.ToString() : "Null";
    //[Obsolete("Use SqLiteral()")] public static string ToSqlValue(this int? @this) => @this.HasValue ? @this.Value.ToString() : "Null";

  }
}