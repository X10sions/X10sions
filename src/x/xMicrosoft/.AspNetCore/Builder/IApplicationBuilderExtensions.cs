using Common.FileProviders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder;
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

  public static IApplicationBuilder UseLocalCdnFileServer(this IApplicationBuilder app, string physicalPath, string requestPath, TimeSpan? maxAge) {
    var fileServerOptions = new FileServerOptions {
      FileProvider = new PhysicalFileProvider(physicalPath),
      RequestPath = new PathString(requestPath),
      EnableDirectoryBrowsing = true
    };
    fileServerOptions.StaticFileOptions.OnPrepareResponse =
      context => context.Context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue {
        MaxAge = maxAge,
        Public = true
      };
    return app.UseFileServer(fileServerOptions);
  }

  public static IApplicationBuilder UseStaticFilesInPagesFolder(this IApplicationBuilder app, IWebHostEnvironment env, IDictionary<string, string> mapping, TimeSpan? maxAge = null, string pagesRoot = "pages") {
    var contentTypeProvider = new FileExtensionContentTypeProvider(mapping);
    var staticFileOptions = new StaticFileOptions {
      ContentTypeProvider = contentTypeProvider,
      FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, pagesRoot)),
      RequestPath = new PathString(""),
      OnPrepareResponse = context => context.Context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue { MaxAge = maxAge, Public = true }
    };
    app.UseStaticFiles(staticFileOptions);
    return app;
  }


}