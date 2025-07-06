using Microsoft.AspNetCore.Http;

namespace Common.AspNetCore.Http;

public class AspNetCoreCookie : IAspNetCoreCookie {
  public string Key { get; set; }
  public string DefaultValue { get; set; }
  public CookieOptions Options { get; set; }
}
