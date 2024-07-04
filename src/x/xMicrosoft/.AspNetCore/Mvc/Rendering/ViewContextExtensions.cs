using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Microsoft.AspNetCore.Mvc.Rendering;
public static class ViewContextExtensions {
  public static string Title(this ViewContext viewContext) => viewContext.ViewData[nameof(Title)]?.ToString();
  public static string ViewDataTitle(this ViewContext viewContext) => viewContext.ViewData.Title();
  public static string ViewDataTitle(this ViewContext viewContext, string newValue) => viewContext.ViewData.Title(newValue);

}