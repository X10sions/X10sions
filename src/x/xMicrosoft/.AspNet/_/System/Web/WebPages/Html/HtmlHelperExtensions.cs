namespace System.Web.WebPages.Html {
  public static class HtmlHelperExtensions {

    public static IHtmlString Attributes(this HtmlHelper htmlHelper, string value, string[] attributeNames) => new HtmlString(string.Join(" ", attributeNames.Select((string x) => $"{x}=\"{value}\"")));

    public static IHtmlString AttributesNameAndId(this HtmlHelper @this, string value) => Attributes(@this, value, new[] {
      "id",
      "name"
    });

  }
}
