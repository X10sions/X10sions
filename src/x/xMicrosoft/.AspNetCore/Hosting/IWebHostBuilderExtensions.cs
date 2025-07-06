using Microsoft.Extensions.Configuration.Json;

namespace Microsoft.AspNetCore.Hosting;

public static class IWebHostBuilderExtensions {
  public static string MapWebRootPath(this IWebHostEnvironment webHostEnvironment, params string[] paths) => Path.Combine(new[] { webHostEnvironment.WebRootPath }.Concat(paths).ToArray());
  public static FileInfo MapWebRootPathFileInfo(this IWebHostEnvironment webHostEnvironment, params string[] paths) => new FileInfo(webHostEnvironment.MapWebRootPath(paths));
  public static DirectoryInfo MapWebRootPathDirectoryInfo(this IWebHostEnvironment webHostEnvironment, string path) => webHostEnvironment.MapWebRootPathFileInfo(path).Directory;
  public static IEnumerable<FileInfo> MapWebRootPathFileInfos(this IWebHostEnvironment webHostEnvironment, string path) => webHostEnvironment.MapWebRootPathDirectoryInfo(path).EnumerateFiles();
  public static IEnumerable<FileInfo> MapWebRootPathFileInfos(this IWebHostEnvironment webHostEnvironment, string path, string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly) => webHostEnvironment.MapWebRootPathDirectoryInfo(path).EnumerateFiles(searchPattern, searchOption);

  public static IWebHostBuilder PrependSharedAppSettings(this IWebHostBuilder builder, FileInfo sharedFile) {
    if (builder == null || sharedFile == null) {
      return builder;
    }
    // modify the config files being used
    builder.ConfigureAppConfiguration((hostingContext, config) => {
      var fileNames = new List<string> {
          sharedFile.Name,
          $"{sharedFile.NameWithoutExtension()}.{hostingContext.HostingEnvironment.EnvironmentName}{sharedFile.Extension}"
        };
      var sharedConfigs = new List<JsonConfigurationSource>();
      // first settings files are the ones in the shared folder that get found when run via dotnet run
      foreach (var fileName in fileNames) {
        var filePath = Path.Combine(sharedFile.Directory.FullName, fileName);
        if (File.Exists(filePath)) {
          sharedConfigs.Add(new JsonConfigurationSource { Path = filePath, Optional = true, ReloadOnChange = true });
        }
      }
      // second settings files are the linked shared settings files found when the site is published
      foreach (var fileName in fileNames) {
        var filePath = Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, fileName);
        if (File.Exists(filePath)) {
          sharedConfigs.Add(new JsonConfigurationSource { Path = filePath, Optional = true, ReloadOnChange = true });
        }
      }
      // create the file providers, since we didn't specify one explicitly
      sharedConfigs.ForEach(x => x.ResolveFileProvider());
      if (config.Sources.Count > 0) {
        for (var idx = 0; idx < sharedConfigs.Count; idx++) { config.Sources.Insert(idx, sharedConfigs[idx]); }
      } else sharedConfigs.ForEach(x => {
        config.Add(x);
      });
      // all other setting files (e.g., appsettings.json) appear afterwards
    });
    return builder;
  }

  public static IWebHostBuilder PrependSharedAppSettingsFromParentDirectory(this IWebHostBuilder builder, string sharedFileName) {
    if (builder == null || string.IsNullOrEmpty(sharedFileName)) {
      return builder;
    }
    // modify the config files being used
    builder.ConfigureAppConfiguration((hostingContext, config) => {
      var parentDir = Directory.GetParent(hostingContext.HostingEnvironment.ContentRootPath);
      var sharedFile = new FileInfo(Path.Combine(parentDir.FullName, sharedFileName));
      builder.PrependSharedAppSettings(sharedFile);
    });
    return builder;
  }

  public static IWebHostBuilder UseSharedAppSettings(this IWebHostBuilder builder, string environmentName, FileInfo sharedAppSettingsFileInfo) {
    builder.UseEnvironment(environmentName ?? EnvironmentName.Development);
    if (environmentName != EnvironmentName.Production) {
      builder.CaptureStartupErrors(true);
      builder.UseSetting("detailedErrors", "true");
    }
    builder.PrependSharedAppSettings(sharedAppSettingsFileInfo);
    return builder;
  }

  //public static IWebHostBuilder UseSharedAppSettings<TStartup>(this IWebHostBuilder builder, string environmentName, FileInfo sharedAppSettingsFileInfo)
  //  => builder.UseSharedAppSettings(environmentName, sharedAppSettingsFileInfo).UseStartup(typeof(TStartup).GetTypeInfo().Assembly.GetName().Name);
  public static string WebRootFilePath(this IWebHostEnvironment webHostEnvironment, string fileName, string fileExtension) => webHostEnvironment.MapWebRootPath($"{fileName}.{fileExtension.TrimStart('.')}");
  public static string WebRootFileEnvironmentPath(this IWebHostEnvironment webHostEnvironment, string fileName, string fileExtension) => webHostEnvironment.WebRootFilePath($"{fileName}.{webHostEnvironment.EnvironmentName}", fileExtension);

}
