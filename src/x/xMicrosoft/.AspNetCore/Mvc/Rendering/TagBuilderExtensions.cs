using System.Text.Encodings.Web;

namespace Microsoft.AspNetCore.Mvc.Rendering {
  public static class TagBuilderExtensions {

    public static string GetString(this TagBuilder content) {
      var writer = new System.IO.StringWriter();
      content.WriteTo(writer, HtmlEncoder.Default);
      return writer.ToString();
    }

  }
}
