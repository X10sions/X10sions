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

    //public static RouteData RouteData(this HttpRequest req) =>  RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
    //public static RouteData RouteData(this HttpRequest req) => req.RequestContext.RouteData;

    public static DateTime GetDateTime(this HttpRequest request, string name) => request[name].As<DateTime>(DateTime.TryParse);
    public static IList<DateTime> GetDateTimeList(this HttpRequest request, string name) => request.GetFormAndQueryStrings(name).ToList<DateTime>(DateTime.TryParse);
    public static IEnumerable<string> GetFormAndQueryStrings(this HttpRequest request, string name) => request.Form.GetValues(name).Union(request.QueryString.GetValues(name));
    public static int GetInt(this HttpRequest request, string name) => request[name].As<int>(int.TryParse);
    public static IList<int> GetIntList(this HttpRequest request, string name) => request.GetFormAndQueryStrings(name).ToList<int>(int.TryParse);
    public static IList<T> GetList<T>(this HttpRequest request, string name, StringExtensions.TryParse<T> parseFunc) => request.GetFormAndQueryStrings(name).ToList(parseFunc);

    public static IEnumerable<string> GetStringList(this HttpRequest request, string name) => from s in request.GetFormAndQueryStrings(name) where !string.IsNullOrWhiteSpace(s) select s.Trim();

    //public static string GetString(this HttpRequest request, string name) => request[name].As<int>(string.TryParse);

    public static T? GetValue<T>(this HttpRequest request, string name, T? defaultValue) => request[name].As(defaultValue);

  }
}
