using System.Net;

namespace Microsoft.AspNetCore.Http {
  public static class IResponseCookiesExtensions {

    public static IResponseCookies Append(this IResponseCookies cookies, Cookie cookie) {
      cookies.Append(cookie.Name, cookie.Value, new CookieOptions {
        Domain = cookie.Domain,
        Expires = cookie.Expires,
        HttpOnly = cookie.HttpOnly,
        //IsEssential = cookie.Comment
        //MaxAge = cookie.max
        Path = cookie.Path,
        //SameSite = cookie.same
        Secure = cookie.Secure
      });
      return cookies;
    }

    public static IResponseCookies Append<T>(this IResponseCookies cookies, string value, T cookieOptions) where T : CookieOptions {
      var type = typeof(T);
      cookies.Append(type.GetFullNameElseName(), value, cookieOptions);
      return cookies;
    }


  }
}
