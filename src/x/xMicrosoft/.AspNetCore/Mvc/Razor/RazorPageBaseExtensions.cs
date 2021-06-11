namespace Microsoft.AspNetCore.Mvc.Razor {
  public static class RazorPageBaseExtensions {
    public static string Title(this RazorPageBase razorPageBase) => razorPageBase.ViewContext.ViewData[nameof(Title)]?.ToString();

  }
}