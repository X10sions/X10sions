using System.Web.Mvc;

namespace System.Web {
  public static class HttpRequestBaseExtensions {
    public static HttpContext Current(this HttpContextBase httpContextBase) => httpContextBase.ApplicationInstance.Context;

    public static bool? GetBoolean(this HttpRequestBase httpContextBase, string key, bool? defaultValue) {
      var text = httpContextBase[key];
      return (text.Length > 0) ? new bool?(Convert.ToBoolean(text)) : defaultValue;
    }

    public static double? GetDouble(this HttpRequestBase httpContextBase, string key, double? defaultValue) {
      var text = httpContextBase[key];
      return text.IsNumeric() ? new double?(Convert.ToDouble(text)) : defaultValue;
    }

    public static int? GetInteger(this HttpRequestBase httpContextBase, string key, int? defaultValue) {
      var text = httpContextBase[key];
      return text.IsNumeric() ? new int?(Convert.ToInt32(text)) : defaultValue;
    }

    public static DateTime? GetDate(this HttpRequestBase httpContextBase, string key, DateTime? defaultValue) {
      var text = httpContextBase[key];
      return text.IsDate() ? new DateTime?(Convert.ToDateTime(text)) : defaultValue;
    }

    public static string GetString(this HttpRequestBase httpContextBase, string key, string defaultValue) {
      var text = httpContextBase[key];
      return (text.Length > 0) ? text : defaultValue;
    }

  }
  public static class HttpContextExtensions {


    public static MvcHtmlString ServerSideInclude(this HttpContext httpContext, string serverPath) {
      var path = httpContext.Server.MapPath(serverPath);
      var value = File.ReadAllText(path);
      return new MvcHtmlString(value);
    }


  }
}
