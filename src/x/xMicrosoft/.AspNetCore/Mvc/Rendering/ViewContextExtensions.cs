namespace Microsoft.AspNetCore.Mvc.Rendering {
  public static class ViewContextExtensions {
    public static string Title(this ViewContext viewContext) => viewContext.ViewData[nameof(Title)]?.ToString();

  }
}