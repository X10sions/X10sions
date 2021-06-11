using Common.FileProviders;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder {
  public static class IApplicationBuilderExtensions {

    public static IDictionary<string, string> DefaultStaticFileMappingDictionary()
      => new Dictionary<string, string> {
        [".css"] = "text/css",
        [".js"] = "application/javascript"
      };

    //public static IApplicationBuilder UseMvcWithAreasDefaultRoute(this IApplicationBuilder builder) => builder.UseEndpoints(endpoints => {
    //  endpoints.MapRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    //  endpoints.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
    //});

    public static IApplicationBuilder UseEmbeddedFileResolvingProviderStaticFiles(this IApplicationBuilder builder, Assembly assembly, string baseNamesapce, string requestPath) {
      builder.UseStaticFiles(new StaticFileOptions {
        FileProvider = new EmbeddedFileResolvingProvider(assembly, baseNamesapce),//"cloudscribe.Web.StaticFiles"
        RequestPath = new PathString(requestPath) //"/cr"
      });
      return builder;
    }

  }
}