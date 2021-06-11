using System.Globalization;

namespace LinqToDB.Tests.Base {
  public static class Helpers {

    public static string ToInvariantString<T>(this T data) => string.Format(CultureInfo.InvariantCulture, "{0}", data).Replace(',', '.').Trim(' ', '.', '0');

  }
}