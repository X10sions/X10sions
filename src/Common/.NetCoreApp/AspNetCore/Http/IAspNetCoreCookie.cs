using Microsoft.AspNetCore.Http;

namespace Common.AspNetcore.Http {
  public interface IAspNetCoreCookie {
    string Key { get; }
    string DefaultValue { get; }
    CookieOptions Options { get; }
  }

  public static class IAspNetCoreCookieExtensions {
    public static string Value(this IAspNetCoreCookie cookie, IRequestCookieCollection cookies) => cookies[cookie.Key] ?? cookie.DefaultValue;
  }

}
