using System.Web.Mvc;

namespace System.Web {
  public static class HttpContextExtensions {
    public static MvcHtmlString ServerSideInclude(this HttpContext httpContext, string serverPath) {
      var path = httpContext.Server.MapPath(serverPath);
      var value = File.ReadAllText(path);
      return new MvcHtmlString(value);
    }
  }
}
