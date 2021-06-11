
using System.Globalization;

namespace System {
  public static class LongExtensions {

    public static string ToInvariant(this long value) => value.ToString(NumberFormatInfo.InvariantInfo);

    [Obsolete] public static string IdString(this long? value) => value.HasValue ? value.ToString() : string.Empty;

  }
}