using Microsoft.Extensions.FileProviders;
using System;

namespace Microsoft.Extensions.Hosting.Internal {
  public static class HostingEnvironmentExtensions {

    public static HostingEnvironment Init(this HostingEnvironment hostingEnvironment) {
      hostingEnvironment.EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      hostingEnvironment.ApplicationName = AppDomain.CurrentDomain.FriendlyName;
      hostingEnvironment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;
      hostingEnvironment.ContentRootFileProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory);
      return hostingEnvironment;
    }

  }
}