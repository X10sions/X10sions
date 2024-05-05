namespace Microsoft.AspNetCore.Mvc.Rendering;
public class AnchorTagBuilder : TagBuilder {
  //public static AnchorTagBuilder Instance<T>(T innerText, string href) => new AnchorTagBuilder(innerText.ToString(), href);

  public AnchorTagBuilder(string innerText, string href) : base("a") {
    MergeAttribute("href", href);
    InnerHtml.Append(innerText);
  }

  //public AnchorTagBuilder(int innerText, string href) : this(innerText.ToString(), href) {    }

  public AnchorTagBuilder(string innerText, IUrlHelper url, string pageName, string? pageHandler = null, object? values = null) : this(innerText, url.Page(pageName, pageHandler, values)) { }
}