using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Extensions.Configuration {
  public static class IConfigurationBuilderExtensions {

    public static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder configurationBuilder, IEnumerable<string> paths, bool optional = true, bool reloadOnChage = true) {
      foreach (var path in paths) {
        configurationBuilder.AddJsonFile(path, optional, reloadOnChage);
        Console.WriteLine($"AddJsonFile:{path} : Exists({File.Exists(path)})");
      }
      return configurationBuilder;
    }

    public static IConfigurationRoot Build(this IConfigurationBuilder configurationBuilder, string jsonFilePath = "appsettings.json", string? basePath = null) {
      if (!string.IsNullOrWhiteSpace(basePath)) {
        configurationBuilder.SetBasePath(basePath);
      }
      configurationBuilder.AddJsonFile(jsonFilePath, optional: false, true);
      return configurationBuilder.Build();
    }

    public static IConfigurationBuilder AddJsonFilesFromJsonConfigurationSection(this IConfigurationBuilder configurationBuilder, string sectionKey, string jsonFilePath = "appsettings.json", string? basePath = null) {
      var tempConfigurationRoot = new ConfigurationBuilder().Build(jsonFilePath, basePath);
      var paths = tempConfigurationRoot.GetSection(sectionKey).Get<string[]>();
      configurationBuilder.AddJsonFiles(paths, true, true);
      //configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
      return configurationBuilder;
    }


  }
}