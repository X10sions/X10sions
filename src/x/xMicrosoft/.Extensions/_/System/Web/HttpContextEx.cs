using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace System.Web {
  [Obsolete("ASP.Net Framework workaround - Use DI instead")]
  public static class HttpContextEx {
    static IHttpContextAccessor _httpContextAccessor;
    [Obsolete("ASP.Net Framework workaround - Use DI instead")] public static void Configure(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;
    [Obsolete("ASP.Net Framework workaround - Use DI instead")] public static Microsoft.AspNetCore.Http.HttpContext Current => _httpContextAccessor.HttpContext;
    //Must be called in StartUp
    [Obsolete("ASP.Net Framework workaround - Use DI instead")] public static void ConfigureHttpContextCurrent(this IApplicationBuilder app) =>Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
  }
}