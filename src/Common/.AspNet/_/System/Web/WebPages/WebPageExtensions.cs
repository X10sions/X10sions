namespace System.Web.WebPages {
  public static class WebPageExtensions {

    public static IHtmlString RenderPageSelectOptions(this WebPage webPage, string path) => RenderPageSelectOptions(webPage, path, new object[] { });

    public static IHtmlString RenderPageSelectOptions<T>(this WebPage webPage, string path, T selectedValue) => RenderPageSelectOptions<T>(webPage, path, new[] { selectedValue });

    public static IHtmlString RenderPageSelectOptions<T>(this WebPage webPage, string path, IEnumerable<T> selectedValues) {
      var renderPageHtml = webPage.RenderPage(path).ToHtmlString();
      foreach (var x in selectedValues) {
        renderPageHtml = renderPageHtml.Replace($"value=\"{x}\"", $"value=\"{x}\" selected=\"true\"");
      }
      return new HtmlString(renderPageHtml);
    }

  }
}
