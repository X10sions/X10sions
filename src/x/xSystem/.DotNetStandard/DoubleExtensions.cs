using System.Globalization;

namespace System {
  public static class DoubleExtensions {
    public static bool IsEven(this double value) => value % 2 == 0;
    public static bool IsOdd(this double value) => value % 2 != 0;
    public static string ToInvariant(this double value) => value.ToString(NumberFormatInfo.InvariantInfo);

  }
}