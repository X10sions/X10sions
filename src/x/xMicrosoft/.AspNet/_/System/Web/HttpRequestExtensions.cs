using System.Web.Mvc;

namespace System.Web {
  public static class HttpRequestExtensions {

    public static string AppRelativeVirtualPath(this HttpRequest httpRequest) => httpRequest.PhysicalPath.Replace(httpRequest.PhysicalApplicationPath.TrimEnd('\\'), string.Empty).Replace("\\", "/");

    public static string FormPostedValues(this HttpRequest httpRequest, string separator = "<br/>") => string.Join(
      separator, from key in httpRequest.Form.AllKeys
                 where !string.IsNullOrWhiteSpace(httpRequest.Form[key])
                 select $"{key}: {httpRequest.Form[key]}{separator}");

    public static bool IsMvcAction(this HttpRequest httpRequest) => httpRequest.RequestContext.RouteData != null;

    public static bool IsPostBackSubmit(this HttpRequest httpRequest, string submitName = "", string submitValue = "") => httpRequest.UrlReferrer == null
        ? false
        : string.Equals(httpRequest.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase)
        && string.Equals(httpRequest.Url.ToString(), httpRequest.UrlReferrer.ToString(), StringComparison.OrdinalIgnoreCase)
        || string.Equals(httpRequest.Url.ToString() + "#", httpRequest.UrlReferrer.ToString(), StringComparison.OrdinalIgnoreCase)
        && string.Equals(httpRequest.Form[submitName], submitValue, StringComparison.OrdinalIgnoreCase);

    //public static string MvcActionName(this HttpRequest req) => IsMvcAction(req) ? RouteData(req).Values["action"].ToString() : "";
    //public static string MvcAreaName(this HttpRequest req) => IsMvcAction(req) ? RouteData(req).DataTokens["area"].ToString() : "";
    //public static string MvcControllerName(this HttpRequest req) => IsMvcAction(req) ? RouteData(req).Values["controller"].ToString() : "";
    public static string MvcAreaName(this HttpRequest httpRequest) => (string)httpRequest.RequestContext.RouteData.DataTokens["Area"] ?? string.Empty;
    public static string MvcActionName(this HttpRequest httpRequest) => (string)httpRequest.RequestContext.RouteData.Values[nameof(Action)] ?? string.Empty;
    public static string MvcControllerName(this HttpRequest httpRequest) => (string)httpRequest.RequestContext.RouteData.Values[nameof(Controller)] ?? string.Empty;

    //public static RouteData RouteData(this HttpRequest req) =>  RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
    //public static RouteData RouteData(this HttpRequest req) => req.RequestContext.RouteData;



  }
}
