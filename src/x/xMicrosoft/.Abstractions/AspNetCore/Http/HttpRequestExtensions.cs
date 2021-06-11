using Microsoft.AspNetCore.Http.Features;
using System;
using System.Linq;

namespace Microsoft.AspNetCore.Http {
  public static class HttpRequestExtensions {
    public static string FirstFormThenQueryValue(this HttpRequest request, string key) => request.Form[key].FirstOrDefault() ?? request.Query[key].FirstOrDefault() ?? string.Empty;
    public static string FirstQueryThenFormValue(this HttpRequest request, string key) => request.Query[key].FirstOrDefault() ?? request.Form[key].FirstOrDefault() ?? string.Empty;

    const string X_Requested_With = "X-Requested-With";
    const string XmlHttpRequest = "XMLHttpRequest";

    public static bool IsAjaxRequest(this HttpRequest request) => request.Headers != null ? request.Headers[X_Requested_With] == XmlHttpRequest : false;

    public static string SchemeHostUrl(this HttpRequest request, string suffix = "") => $"{request.Scheme}://{request.Host}{suffix}";
    public static string SchemeHostPathUrl(this HttpRequest request, string suffix = "") => $"{request.SchemeHostUrl()}{request.PathBase}{request.Path}{suffix}";

    public static string GetBaseUrl(this HttpRequest request, string? pathBase = null) => $"{request.Scheme}://{request.Host.ToUriComponent()}{pathBase ?? request.PathBase.ToUriComponent()}";

    public static string GetRawTarget(this HttpRequest request) => request.HttpContext.Features.GetHttpRequestFeature().RawTarget;

    public static UriBuilder UriBuilder(HttpRequest request, string userName = "", string password = "", string fragment = "") =>
      new UriBuilder(request.Scheme, request.Host.Value) {
        //      Scheme = request.Scheme,
        //      Host = request.Host.Host,
        //      Port = request.Host.Port.Value,
        UserName = userName,
        Password = password,
        Path = request.Path,
        Query = request.QueryString.Value,
        Fragment = fragment
      };

  }
}
