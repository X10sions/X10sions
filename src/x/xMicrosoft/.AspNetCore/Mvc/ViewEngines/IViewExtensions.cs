using System.Text;

namespace Microsoft.AspNetCore.Mvc.ViewEngines {
  public static class IViewExtensions {


    public static string DebugString(this IView view, bool includeChildren = false) {
      var sb = new StringBuilder();
      sb.AppendLine($"  IView:");
      sb.AppendLine($"    .Path: {view.Path}");
      return sb.ToString();
    }

  }
}
