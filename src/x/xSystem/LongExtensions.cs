using System.Globalization;

namespace System {
  public static class LongExtensions {

    public static string ToInvariant(this long value) => value.ToString(NumberFormatInfo.InvariantInfo);

  }
}