using Common.Helpers;
using System.Collections;

namespace System.Web {
  public static class HttpContextExtensions {

    public static AssetHelper? GetAssetsHelper(this HttpContext httpContext) => httpContext.Items.GetOrCreate(() => new AssetHelper());

    public static IHtmlString ServerSideInclude(this HttpContext httpContext, string serverPath) {
      var path = httpContext.Server.MapPath(serverPath);
      var value = File.ReadAllText(path);
      return new HtmlString(value);
    }

  }
}
