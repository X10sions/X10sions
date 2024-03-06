using System.Web.Mvc;

namespace System.Web {
  public static class HttpRequestExtensions {
    public static string MvcControllerName(this HttpRequest httpRequest) => (string)httpRequest.RequestContext.RouteData.Values[nameof(Controller)] ?? string.Empty;
  }
}
