using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Microsoft.AspNetCore.Mvc.Razor;
public static class IRazorPageExtensions {

  public static string ViewDataTitle(this IRazorPage razorPage) => razorPage.ViewContext.ViewData.Title();
  public static string ViewDataTitle(this IRazorPage razorPage, string newValue) => razorPage.ViewContext.ViewData.Title(newValue);

}