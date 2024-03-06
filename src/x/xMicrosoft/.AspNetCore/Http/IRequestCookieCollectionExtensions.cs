using System.Net;

namespace Microsoft.AspNetCore.Http {
  public static class IRequestCookieCollectionExtensions {

    public static string? Get<T>(this IRequestCookieCollection cookies) where T : CookieOptions => cookies[typeof(T).FullName ?? typeof(T).Name];

    public static string? Get(this IRequestCookieCollection cookies, Cookie cookie) => cookies[cookie.Name];

  }
}