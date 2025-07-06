using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Microsoft.AspNetCore.Mvc.RazorPages {
  public static class PageExtensions {
    public static string ViewDataTitle(this Page page) => page.ViewContext.ViewData.Title();
    public static string ViewDataTitle(this Page page, string newValue) => page.ViewContext.ViewData.Title(newValue);

  }
}