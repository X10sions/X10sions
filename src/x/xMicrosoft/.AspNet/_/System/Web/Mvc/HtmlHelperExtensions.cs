using System.Net;
using System.Web.Routing;

namespace System.Web.Mvc {
  public static class HtmlHelperExtensions {

    public static string GetRequestLayout(this HtmlHelper helper) {
      var httpContext = helper.ViewContext.RequestContext.HttpContext;
      return httpContext.Request[nameof(WebPages.WebPageBase.Layout)];
    }

    public static string GetSessionLayout(this HtmlHelper helper) {
      var httpContext = helper.ViewContext.RequestContext.HttpContext;
      return httpContext.Session[nameof(WebPages.WebPageBase.Layout)].ToString();
    }

    public static IDictionary<string, object> MergeHtmlAttributes(this HtmlHelper helper, object htmlAttributesObject, object defaultHtmlAttributesObject) {
      var source = new string[] { "class" };
      var dictionary = htmlAttributesObject as IDictionary<string, object>;
      var dictionary2 = defaultHtmlAttributesObject as IDictionary<string, object>;
      var routeValueDictionary = (dictionary != null) ? new RouteValueDictionary(dictionary) : HtmlHelper.AnonymousObjectToHtmlAttributes((htmlAttributesObject));
      var routeValueDictionary2 = (dictionary2 != null) ? new RouteValueDictionary(dictionary2) : HtmlHelper.AnonymousObjectToHtmlAttributes((defaultHtmlAttributesObject));
      foreach (var item in routeValueDictionary) {
        if (source.Contains(item.Key)) {
          routeValueDictionary2[item.Key] = (routeValueDictionary2[item.Key] != null) ? $"{routeValueDictionary2[item.Key]} {item.Value}" : item.Value;
        } else {
          routeValueDictionary2[item.Key] = item.Value;
        }
      }
      return routeValueDictionary2;
    }

    public static MvcHtmlString ServerSideInclude(this HtmlHelper htmlHelper, string serverPath) => HttpContext.Current.ServerSideInclude(serverPath);

    public static void SetSessionLayout(this HtmlHelper helper, string layout) {
      var httpContext = helper.ViewContext.RequestContext.HttpContext;
      httpContext.Session[nameof(WebPages.WebPageBase.Layout)] = layout;
    }

    public static MvcHtmlString WebPage(this HtmlHelper htmlHelper, string url) => MvcHtmlString.Create(new WebClient().DownloadString(url));

  }
}
