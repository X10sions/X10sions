using Common.AspNetcore.Http;

namespace Microsoft.AspNetCore.Http {
  public static class IRequestCookieCollectionExtensions {
    public static string Get(this IRequestCookieCollection cookies, IAspNetCoreCookie cookie) => cookie.Value(cookies);
    //public static string Get<T>(this IRequestCookieCollection cookies) where T : IAspNetCoreCookie => cookies[(T)default.xKey];
  }
  public static class IResponseCookiesExtensions {
    public static IResponseCookies Append(this IResponseCookies cookies, IAspNetCoreCookie cookie) {
      cookies.Append(cookie.Key, cookie.DefaultValue, cookie.Options);
      return cookies;
    }
  }
}