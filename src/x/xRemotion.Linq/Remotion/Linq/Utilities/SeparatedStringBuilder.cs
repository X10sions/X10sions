using System;
using System.Collections.Generic;
using System.Text;

namespace Remotion.Linq.Utilities {
  /// <summary>
  /// Builds a string from a sequence, separating each item with a given separator string.
  /// </summary>
  [Obsolete]
  public static class SeparatedStringBuilder {
    public static string Build<T>(string separator, IEnumerable<T> sequence) {
      var sb = new StringBuilder();
      var first = true;
      foreach (var item in sequence) {
        if (!first)
          sb.Append(separator);
        sb.Append(item);

        first = false;
      }
      return sb.ToString();
    }
  }

}