using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Microsoft.AspNetCore.Mvc.RazorPages {
  public static class PageBaseExtensions {
    public static string ViewDataTitle(this PageBase page) => page.ViewContext.ViewData.Title();
    public static string ViewDataTitle(this PageBase page, string newValue) => page.ViewContext.ViewData.Title(newValue);
  }
}