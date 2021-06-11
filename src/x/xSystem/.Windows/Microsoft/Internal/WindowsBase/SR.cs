using System.Globalization;
using System.Resources;

namespace Microsoft.Internal.WindowsBase {
  internal static class SR {
    private static ResourceManager _resourceManager = new ResourceManager("ExceptionStringTable", typeof(SR).Assembly);

    internal static ResourceManager ResourceManager => _resourceManager;

    internal static string Get(string id) {
      string @string = _resourceManager.GetString(id);
      if (@string == null) {
        @string = _resourceManager.GetString("Unavailable");
      }
      return @string;
    }

    internal static string Get(string id, params object[] args) {
      string text = _resourceManager.GetString(id);
      if (text == null) {
        text = _resourceManager.GetString("Unavailable");
      } else if (args != null && args.Length != 0) {
        text = string.Format(CultureInfo.CurrentCulture, text, args);
      }
      return text;
    }
  }

}